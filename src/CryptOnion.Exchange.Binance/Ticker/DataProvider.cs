using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using CryptOnion.Currency;
using CryptOnion.Currency.Finder;
using CryptOnion.Observable;

namespace CryptOnion.Exchange.Binance.Ticker
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

        public record TickObj(string symbol, string askPrice, string bidPrice, string lowPrice, string highPrice,
                    string weightedAvgPrice, string volume, string lastPrice, string priceChange, string priceChangePercent);

        public async Task<IEnumerable<CryptOnion.Ticker>> ProvideAsync(CancellationToken cancellationToken)
        {
            var responseMessage = await this._httpClient.GetAsync("https://www.binance.com/api/v3/ticker/24hr", HttpCompletionOption.ResponseHeadersRead);
            responseMessage.EnsureSuccessStatusCode();

            using var stream = await responseMessage.Content.ReadAsStreamAsync();

            var jsonObjs = await JsonSerializer.DeserializeAsync<IEnumerable<TickObj>>(stream);

            var result = jsonObjs.Select(tickObj =>
            {
                var pair = this.TryParse(tickObj.symbol);

                if (pair != null)
                {
                    var ticker = new CryptOnion.Ticker(pair);

                    Decimal.TryParse(tickObj.askPrice, out decimal lowestAsk);
                    ticker.LowestAsk = lowestAsk;

                    Decimal.TryParse(tickObj.bidPrice, out decimal bidPrice);
                    ticker.HighestBid = bidPrice;

                    Decimal.TryParse(tickObj.priceChange, out decimal priceChange);
                    ticker.Change = priceChange;

                    Decimal.TryParse(tickObj.lastPrice, out decimal last);
                    ticker.Last = last;

                    Decimal.TryParse(tickObj.volume, out decimal volume);
                    ticker.Volume = volume;

                    return ticker;
                }

                return null;
            })
            .Where(t => t != null);

            return result;
        }

        private Pair TryParse(string pair)
        {
            if (string.IsNullOrEmpty(pair))
            {
                return null;
            }

            var clearPair = string.Join("",
                pair.Where(c => char.IsLetterOrDigit(c))
                    .Select(char.ToUpperInvariant)
            );

            if (clearPair.Length < 6)
            {
                return null;
            }

            for (int i = 2; i < clearPair.Length - 3; i++)
            {
                var firstCurr = clearPair.Substring(0, i + 1);
                var secCurr = clearPair.Substring(i + 1);

                var fCurr = this._currencyFinder.Find(firstCurr);
                var sCurr = this._currencyFinder.Find(secCurr);

                if (fCurr != null && sCurr != null)
                {
                    return new Pair(fCurr, sCurr);
                }
            }

            return null;
        }
    }
}
