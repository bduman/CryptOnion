using System;
using System.Collections.Generic;

namespace CryptOnion.Exchange
{
    public class ExchangeBase
    {
        private readonly Dictionary<Type, object> _scheduledObservables;
        public string Name { get; private set; }

        protected readonly ICurrencyFinder CurrencyFinder;

        public ExchangeBase(string name, ICurrencyFinder currencyFinder)
        {
            this.Name = name;
            this.CurrencyFinder = currencyFinder;
            this._scheduledObservables = new Dictionary<Type, object>();
        }

        protected void AddScheduledObservable<T>(ScheduledObservable<T> scheduledObservable)
        {
            this._scheduledObservables.Add(typeof(T), scheduledObservable);
        }

        public ScheduledObservable<T> GetScheduledObservable<T>()
        {
            return this._scheduledObservables.GetValueOrDefault(typeof(T)) as ScheduledObservable<T>;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
