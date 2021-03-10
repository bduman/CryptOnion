using System;

namespace CryptOnion.Exchange
{
    public interface IExchange
    {
        IObservable<T> GetObservable<T>();
    }
}
