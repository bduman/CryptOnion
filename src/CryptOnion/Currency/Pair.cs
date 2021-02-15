using CryptOnion.Currency;

namespace CryptOnion
{
    public sealed class Pair
    {
        public Pair(CurrencyBase baseCurrency, CurrencyBase quoteCurrency)
        {
            this.BaseCurrency = baseCurrency;
            this.QuoteCurrency = quoteCurrency;
        }

        public CurrencyBase BaseCurrency { get; }
        public CurrencyBase QuoteCurrency { get; }

        public override string ToString()
        {
            return this.BaseCurrency.ToString() + "_" + this.QuoteCurrency.ToString();
        }
    }
}