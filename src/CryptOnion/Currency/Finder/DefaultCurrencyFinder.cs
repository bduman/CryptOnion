using System.Collections.Generic;
using CryptOnion.Currency;

namespace CryptOnion
{
    public class DefaultCurrencyFinder : ICurrencyFinder
    {
        private readonly Dictionary<string, CurrencyBase> _currencies;

        public DefaultCurrencyFinder()
        {
            this._currencies = new Dictionary<string, CurrencyBase>();
        }

        public void Add(CurrencyBase currency)
        {
            if (!this._currencies.ContainsKey(currency.Code))
            {
                this._currencies.Add(currency.Code, currency);
            }
        }

        public CurrencyBase Find(string code)
        {
            return this._currencies.GetValueOrDefault(code);
        }
    }
}
