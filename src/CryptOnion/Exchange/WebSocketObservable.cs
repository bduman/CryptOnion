using System;
using System.Net.WebSockets;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Microsoft.IO;

namespace CryptOnion.Exchange
{
    public abstract class WebSocketObservable : IObservable<byte>
    {
        private readonly IObservable<byte> _source;
        private const int _chunkSize = 4 * 5; // todo: increase it

        protected readonly static RecyclableMemoryStreamManager _memoryStreamManager = new RecyclableMemoryStreamManager()
        {
            ThrowExceptionOnToArray = true
        };

        public WebSocketObservable(Uri uri)
        {
            var source = Observable.Using<byte, ClientWebSocket>(async (token) =>
            {
                var webSocket = new ClientWebSocket();
                await webSocket.ConnectAsync(uri, token).ConfigureAwait(false);

                return webSocket;
            },
            (webSocket, token) =>
            {
                var buffer = WebSocket.CreateClientBuffer(_chunkSize, _chunkSize);

                var observable = Observable.Create<byte>(async (observer, subToken) =>
                {
                    while (webSocket.State == WebSocketState.Open && !subToken.IsCancellationRequested)
                    {
                        WebSocketReceiveResult result;

                        do
                        {
                            result = await webSocket.ReceiveAsync(buffer, subToken);

                            for (int i = buffer.Offset; i < result.Count; i++)
                            {
                                observer.OnNext(buffer[i]);
                            }
                        }
                        while (!result.EndOfMessage);

                        if (result.MessageType == WebSocketMessageType.Text)
                        {
                            observer.OnNext(byte.MinValue);
                        }
                    }
                });

                return Task.FromResult(observable);
            });

            this._source = source
                .ObserveOn(Scheduler.Default)
                .SubscribeOn(Scheduler.Default)
                .Publish().RefCount();
        }

        public IDisposable Subscribe<TResult>(IObserver<TResult> observer,
            Func<IObservable<byte>, IObservable<TResult>> resultSelector)
        {
            return resultSelector(this._source).Subscribe(observer);
        }

        public IDisposable Subscribe(IObserver<byte> observer)
        {
            return this.Subscribe(observer, (o) => o);
        }
    }
}
