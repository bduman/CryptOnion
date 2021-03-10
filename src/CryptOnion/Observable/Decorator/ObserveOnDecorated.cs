using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace CryptOnion.Observable.Decorator
{
    public class ObserveOnDecorated<T> : IObservable<T>
    {
        private readonly IObservable<T> _decorated;

        public ObserveOnDecorated(IObservable<T> decorated, IScheduler scheduler)
        {
            this._decorated = decorated.ObserveOn(scheduler);
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            return this._decorated.Subscribe(observer);
        }
    }
}
