using System;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace CryptOnion.Observable.WebSocket
{
    public class Observable : IObservable<byte>
    {
        private readonly IDataProvider<ClientWebSocket> _dataProvider;
        private readonly IObservable<byte> _observable;
        private const int _chunkSize = 128;

        public Observable(IDataProvider<ClientWebSocket> dataProvider)
        {
            this._dataProvider = dataProvider;

            this._observable = System.Reactive.Linq.Observable.Using<byte, ClientWebSocket>(
                async (token) => await this._dataProvider.ProvideAsync(token),
                (webSocket, token) =>
                {
                    var buffer = System.Net.WebSockets.WebSocket.CreateClientBuffer(_chunkSize, _chunkSize);

                    var observable = System.Reactive.Linq.Observable.Create<byte>(async (observer, subToken) =>
                    {
                        System.Console.WriteLine("Observable ThreadId: " + System.Threading.Thread.CurrentThread.ManagedThreadId);
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
        }

        public IDisposable Subscribe(IObserver<byte> observer)
        {
            System.Console.WriteLine("Subscribe ThreadId: " + System.Threading.Thread.CurrentThread.ManagedThreadId);
            return this._observable.Subscribe(observer);
        }
    }
}