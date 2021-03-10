using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using CryptOnion.Observable;

namespace CryptOnion.Exchange.Paribu.Ticker
{
    public class DataProvider : IDataProvider<IEnumerable<CryptOnion.Ticker>>
    {
        private readonly HttpClient _httpClient;
        private readonly ICurrencyFinder _currencyFinder;

        public DataProvider(ICurrencyFinder currencyFinder, HttpClient httpClient)
        {
            this._httpClient = httpClient;
            this._currencyFinder = currencyFinder;
        }

        public record TickObj(decimal lowestAsk, decimal highestBid, decimal low24hr, decimal high24hr,
            decimal avg24hr, decimal volume, decimal last, decimal change, decimal percentChange);

        public async Task<IEnumerable<CryptOnion.Ticker>> ProvideAsync(CancellationToken cancellationToken)
        {
            var responseMessage = await this._httpClient.GetAsync("https://www.paribu.com/ticker", HttpCompletionOption.ResponseHeadersRead);
            responseMessage.EnsureSuccessStatusCode();

            using var stream = await responseMessage.Content.ReadAsStreamAsync();

            var jsonObj = await JsonSerializer.DeserializeAsync<Dictionary<string, TickObj>>(stream);

            var result = jsonObj.Select(tickObj =>
            {
                var curs = tickObj.Key.Split('_');
                var baseCur = this._currencyFinder.Find(curs[0]);
                var quoteCurr = this._currencyFinder.Find(curs[1]);

                if (baseCur != null && quoteCurr != null)
                {
                    return new CryptOnion.Ticker(new Pair(baseCur, quoteCurr))
                    {
                        LowestAsk = tickObj.Value.lowestAsk,
                        HighestBid = tickObj.Value.highestBid,
                        Change = tickObj.Value.change,
                        Last = tickObj.Value.last,
                        Volume = tickObj.Value.volume
                    };
                }

                return null;
            }).Where(t => t != null);

            return result;
        }
    }
}
