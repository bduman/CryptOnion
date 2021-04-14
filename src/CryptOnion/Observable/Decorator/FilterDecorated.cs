using System;
using System.Reactive.Linq;
using CryptOnion.Currency.Filter;

namespace CryptOnion.Observable.Decorator
{
    public class FilterDecorated<T> : IObservable<T>
    {
        private readonly IFilterHandler<T> _filterHandler;
        private readonly IObservable<T> _decorated;

        public FilterDecorated(IObservable<T> decorated, IFilterHandler<T> filterHandler)
        {
            this._filterHandler = filterHandler;
            this._decorated = decorated.Where(Filters);
        }

        private bool Filters(T obj)
        {
            foreach (var filter in this._filterHandler.GetFilters())
            {
                if (!filter.IsPassed(obj))
                {
                    return false;
                }
            }

            return true;
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            return this._decorated.Subscribe(observer);
        }
    }
}
