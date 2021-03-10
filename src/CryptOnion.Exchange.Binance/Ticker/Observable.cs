using System.Collections.Generic;
using CryptOnion.Observable;
using CryptOnion.Observable.Scheduled;

namespace CryptOnion.Exchange.Binance.Ticker
{
    public class Observable : Observable<CryptOnion.Ticker>
    {
        public Observable(IDataProvider<IEnumerable<CryptOnion.Ticker>> provider, IInternalDelay internalDelay) : base(provider, internalDelay)
        {

        }
    }
}
