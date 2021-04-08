namespace CryptOnion.Currency.Finder
{
    public interface ICurrencyFinder
    {
        CurrencyBase Find(string code);
    }
}
