using CryptOnion.Currency;

namespace CryptOnion
{
    public sealed class Pair
    {
        public Pair(AbstractCurrency baseCurrency, AbstractCurrency quoteCurrency)
        {
            this.BaseCurrency = baseCurrency;
            this.QuoteCurrency = quoteCurrency;
        }

        public AbstractCurrency BaseCurrency { get; }
        public AbstractCurrency QuoteCurrency { get; }

        public override string ToString()
        {
            return this.BaseCurrency.ToString() + "_" + this.QuoteCurrency.ToString();
        }
    }
}