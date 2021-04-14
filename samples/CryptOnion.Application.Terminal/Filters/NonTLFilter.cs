using CryptOnion.Currency.Filter;

namespace CryptOnion.Application.Terminal.Filters
{
    public class NonTLFilter : IFilter<Ticker>
    {
        public bool IsPassed(Ticker obj)
        {
            var isTL = obj.Pair.BaseCurrency.Code == "TL" || obj.Pair.QuoteCurrency.Code == "TL";

            return isTL;
        }
    }
}
