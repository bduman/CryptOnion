using System.Collections.Generic;

namespace CryptOnion.Currency.Finder
{
    class DefaultCurrencyFinder : ICurrencyFinder
    {
        private readonly Dictionary<string, CurrencyBase> _currencies;

        public DefaultCurrencyFinder(IEnumerable<CurrencyBase> currencies)
        {
            this._currencies = new Dictionary<string, CurrencyBase>();
            this.AddRange(currencies);
        }

        private void AddRange(IEnumerable<CurrencyBase> currencies)
        {
            foreach (var currency in currencies)
            {
                this.Add(currency);
            }
        }

        private void Add(CurrencyBase currency)
        {
            if (!this._currencies.ContainsKey(currency.Code))
            {
                this._currencies.Add(currency.Code, currency);
            }
        }

        public CurrencyBase Find(string code)
        {
            return this._currencies.GetValueOrDefault(code) ?? new CurrencyBase(code, code);
        }
    }
}
