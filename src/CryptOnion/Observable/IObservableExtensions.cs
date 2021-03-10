using System;

namespace CryptOnion.Observable
{
    static class IObservableExtensions
    {
        static IDisposable Subscribe<T, TResult>(this IObservable<T> observable, IObserver<TResult> observer,
            Func<IObservable<T>, IObservable<TResult>> resultSelector)
        {
            return resultSelector(observable).Subscribe(observer);
        }
    }
}
