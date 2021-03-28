using System.Collections.Generic;
using CryptOnion.Currency;

namespace CryptOnion
{
    public class DefaultCurrencyFinder : ICurrencyFinder
    {
        private readonly Dictionary<string, AbstractCurrency> _currencies;

        public DefaultCurrencyFinder()
        {
            this._currencies = new Dictionary<string, AbstractCurrency>();
        }

        public void Add(AbstractCurrency currency)
        {
            if (!this._currencies.ContainsKey(currency.Code))
            {
                this._currencies.Add(currency.Code, currency);
            }
        }

        public AbstractCurrency Find(string code)
        {
            return this._currencies.GetValueOrDefault(code);
        }
    }
}
