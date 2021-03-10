using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptOnion.Observable.Scheduled
{
    public class Observable<T> : IObservable<T>
    {
        private readonly IObservable<T> _observable;

        public Observable(IDataProvider<IEnumerable<T>> dataProvider, IInternalDelay internalDelay)
        {
            this._observable = System.Reactive.Linq.Observable.Create<T>(async (o, token) =>
            {
                while (true)
                {
                    var data = await dataProvider.ProvideAsync(token);

                    foreach (var t in data)
                    {
                        o.OnNext(t);
                    }

                    await Task.Delay(internalDelay.GetDelay());
                }
            });
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            return this._observable.Subscribe(observer);
        }
    }
}
