using System.Collections.Generic;
using System.Linq;

namespace CryptOnion.Currency.Filter
{
    class GenericFilterHandler<T> : IFilterHandler<T>
    {
        private readonly List<IFilter<T>> _filters;

        public GenericFilterHandler(IEnumerable<IFilter<T>> defaultFilters)
        {
            this._filters = defaultFilters.ToList();
        }

        public void Add(IFilter<T> filter)
        {
            this._filters.Add(filter);
        }

        public IEnumerable<IFilter<T>> GetFilters()
        {
            return this._filters;
        }

        public void Remove(IFilter<T> filter)
        {
            this._filters.Remove(filter);
        }
    }
}
