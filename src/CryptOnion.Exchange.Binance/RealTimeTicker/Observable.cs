using System.Net.WebSockets;
using CryptOnion.Observable;

namespace CryptOnion.Exchange.Binance.RealTimeTicker
{
    public class Observable : CryptOnion.Observable.WebSocket.Observable
    {
        public Observable(IDataProvider<ClientWebSocket> dataProvider) : base(dataProvider)
        {

        }
    }
}
