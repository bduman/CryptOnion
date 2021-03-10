using System;
using System.Reactive.Linq;

namespace CryptOnion.Observable.Decorator
{
    public class ShareDecorated<T> : IObservable<T>
    {
        private readonly IObservable<T> _decorated;

        public ShareDecorated(IObservable<T> decorated)
        {
            this._decorated = decorated.Publish().RefCount();
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            return this._decorated.Subscribe(observer);
        }
    }
}
