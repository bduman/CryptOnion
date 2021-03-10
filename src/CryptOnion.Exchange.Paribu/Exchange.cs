using System;
using System.Collections.Generic;

namespace CryptOnion.Exchange.Paribu
{
    public class Exchange : IExchange
    {
        private readonly Dictionary<Type, object> _observables;

        public Exchange(Ticker.Observable tickerObservable)
        {
            this.AddObservable(tickerObservable);
        }

        private void AddObservable<T>(IObservable<T> observable)
        {
            this._observables.Add(typeof(T), observable);
        }

        public IObservable<T> GetObservable<T>()
        {
            return this._observables.GetValueOrDefault(typeof(T)) as IObservable<T>;
        }
    }
}