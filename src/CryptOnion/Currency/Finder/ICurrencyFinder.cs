using CryptOnion.Currency;

namespace CryptOnion
{
    public interface ICurrencyFinder
    {
        AbstractCurrency Find(string code);
    }
}
