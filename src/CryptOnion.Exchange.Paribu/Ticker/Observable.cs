using System.Collections.Generic;
using CryptOnion.Observable;
using CryptOnion.Observable.Scheduled;

namespace CryptOnion.Exchange.Paribu.Ticker
{
    public class Observable : Observable<CryptOnion.Ticker>
    {
        public Observable(IDataProvider<IEnumerable<CryptOnion.Ticker>> dataProvider, IInternalDelay internalDelay) : base(dataProvider, internalDelay)
        {
        }
    }
}
