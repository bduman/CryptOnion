using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace CryptOnion.Observable.Decorator
{
    public class SubscribeOnDecorated<T> : IObservable<T>
    {
        private readonly IObservable<T> _decorated;

        public SubscribeOnDecorated(IObservable<T> decorated, IScheduler scheduler)
        {
            this._decorated = decorated.SubscribeOn(scheduler);
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            return this._decorated.Subscribe(observer);
        }
    }
}
