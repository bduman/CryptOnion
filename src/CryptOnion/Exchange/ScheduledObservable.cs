using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace CryptOnion.Exchange
{
    public abstract class ScheduledObservable<T> : IObservable<T>
    {
        private readonly IObservable<T> _source;

        public ScheduledObservable(TimeSpan internalDelay)
        {
            var source = Observable.Create<T>(async (o) =>
            {
                while (true)
                {
                    await foreach (var t in this.GetAsyncEnumerable())
                    {
                        o.OnNext(t);
                    }

                    await Task.Delay(internalDelay);
                }
            });

            this._source = source
                .ObserveOn(Scheduler.Default)
                .SubscribeOn(Scheduler.Default)
                .Publish().RefCount();
        }

        public IDisposable Subscribe<TResult>(IObserver<TResult> observer,
            Func<IObservable<T>, IObservable<TResult>> resultSelector)
        {
            return resultSelector(this._source).Subscribe(observer);
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            return this.Subscribe(observer, (o) => o);
        }

        protected abstract IAsyncEnumerable<T> GetAsyncEnumerable();
    }
}
