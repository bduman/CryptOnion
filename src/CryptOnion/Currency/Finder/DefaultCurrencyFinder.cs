using System.Collections.Generic;
using CryptOnion.Currency;

namespace CryptOnion
{
    class DefaultCurrencyFinder : ICurrencyFinder
    {
        private readonly Dictionary<string, AbstractCurrency> _currencies;

        internal DefaultCurrencyFinder(IEnumerable<AbstractCurrency> currencies)
        {
            this._currencies = new Dictionary<string, AbstractCurrency>();
            this.AddRange(currencies);
        }

        private void AddRange(IEnumerable<AbstractCurrency> currencies)
        {
            foreach (var currency in currencies)
            {
                this.Add(currency);
            }
        }

        private void Add(AbstractCurrency currency)
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
