using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;

namespace CryptOnion.Exchange.Binance
{
    public class SOTicker : ScheduledObservable<Ticker>
    {
        private readonly ICurrencyFinder _currencyFinder;
        private readonly HttpClient _client;

        public SOTicker(ICurrencyFinder currencyFinder, HttpClient client) : base(TimeSpan.FromSeconds(3))
        {
            this._currencyFinder = currencyFinder;
            this._client = client;
        }

        public record TickObj(string symbol, string askPrice, string bidPrice, string lowPrice, string highPrice,
                    string weightedAvgPrice, string volume, string lastPrice, string priceChange, string priceChangePercent);

        protected async override IAsyncEnumerable<Ticker> GetAsyncEnumerable()
        {
            var responseMessage = await this._client.GetAsync("https://www.binance.com/api/v3/ticker/24hr", HttpCompletionOption.ResponseHeadersRead);
            responseMessage.EnsureSuccessStatusCode();

            using var stream = await responseMessage.Content.ReadAsStreamAsync();

            var jsonObjs = await JsonSerializer.DeserializeAsync<IEnumerable<TickObj>>(stream);

            foreach (var tickObj in jsonObjs)
            {
                var pair = this.TryParse(tickObj.symbol);

                if (pair != null)
                {
                    var ticker = new Ticker(pair);

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

                    yield return ticker;
                }
            }
        }

        public Pair TryParse(string pair)
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

                var fCurr = this._currencyFinder.Find(firstCurr.ToString());
                var sCurr = this._currencyFinder.Find(secCurr.ToString());

                if (fCurr != null && sCurr != null)
                {
                    return new Pair(fCurr, sCurr);
                }
            }

            return null;
        }
    }
}
