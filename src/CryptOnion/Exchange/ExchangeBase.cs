using System;
using System.Collections.Generic;

namespace CryptOnion.Exchange
{
    public abstract class ExchangeBase
    {
        private readonly Dictionary<Type, object> _scheduledObservables;
        private readonly Dictionary<Type, object> _websocketObservables;
        public string Name { get; private set; }

        protected readonly ICurrencyFinder CurrencyFinder;

        public ExchangeBase(string name, ICurrencyFinder currencyFinder)
        {
            this.Name = name;
            this.CurrencyFinder = currencyFinder;
            this._scheduledObservables = new Dictionary<Type, object>();
            this._websocketObservables = new Dictionary<Type, object>();
        }

        protected void AddScheduledObservable<T>(ScheduledObservable<T> scheduledObservable)
        {
            this._scheduledObservables.Add(typeof(T), scheduledObservable);
        }

        public ScheduledObservable<T> GetScheduledObservable<T>()
        {
            return this._scheduledObservables.GetValueOrDefault(typeof(T)) as ScheduledObservable<T>;
        }

        protected void AddWebSocketObservable<T>(WebSocketObservable scheduledObservable)
        {
            this._websocketObservables.Add(typeof(T), scheduledObservable);
        }

        public WebSocketObservable GetWebSocketObservable<T>()
        {
            return this._websocketObservables.GetValueOrDefault(typeof(T)) as WebSocketObservable;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
