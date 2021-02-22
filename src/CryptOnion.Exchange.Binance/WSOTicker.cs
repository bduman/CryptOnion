using System;

namespace CryptOnion.Exchange.Binance
{
    public class WSOTicker : WebSocketObservable
    {
        public WSOTicker() : base(new Uri("wss://stream.binance.com:9443/ws/bnbbtc@ticker"))
        {

        }
    }
}
