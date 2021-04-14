using System.Collections.Generic;

namespace CryptOnion.Currency.Filter
{
    public interface IFilterHandler<T>
    {
        void Add(IFilter<T> filter);
        void Remove(IFilter<T> filter);
        IEnumerable<IFilter<T>> GetFilters();
    }
}
