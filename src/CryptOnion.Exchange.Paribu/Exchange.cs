using System.Net.Http;

namespace CryptOnion.Exchange.Paribu
{
    public sealed class Exchange : ExchangeBase
    {
        public Exchange(ICurrencyFinder currencyFinder, HttpClient httpClient) : base(name: "Paribu", currencyFinder)
        {
            this.AddScheduledObservable(new OTicker(currencyFinder, httpClient));
        }
    }
}