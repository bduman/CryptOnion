using CryptOnion.Currency;

namespace CryptOnion
{
    public interface ICurrencyFinder
    {
        CurrencyBase Find(string code);
    }
}
