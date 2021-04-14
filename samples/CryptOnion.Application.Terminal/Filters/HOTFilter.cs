using CryptOnion.Currency.Filter;

namespace CryptOnion.Application.Terminal.Filters
{
    public class HOTFilter : IFilter<Ticker>
    {
        public bool IsPassed(Ticker obj)
        {
            var isHOT = obj.Pair.BaseCurrency.Code == "HOT";

            return !isHOT;
        }
    }
}
