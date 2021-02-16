using System.Net.Http;

namespace CryptOnion.Exchange.Binance
{
    public class Exchange : ExchangeBase
    {
        public Exchange(ICurrencyFinder currencyFinder, HttpClient httpClient) : base("Binance", currencyFinder)
        {
            this.AddScheduledObservable(new OTicker(currencyFinder, httpClient));
        }
    }
}
