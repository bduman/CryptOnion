using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;

namespace CryptOnion.Exchange.Paribu
{
    internal class OTicker : ScheduledObservable<Ticker>
    {
        private readonly ICurrencyFinder _currencyFinder;
        private readonly HttpClient _client;

        public OTicker(ICurrencyFinder currencyFinder, HttpClient client) : base(TimeSpan.FromSeconds(3))
        {
            this._currencyFinder = currencyFinder;
            this._client = client;
        }

        public record TickObj(decimal lowestAsk, decimal highestBid, decimal low24hr, decimal high24hr,
            decimal avg24hr, decimal volume, decimal last, decimal change, decimal percentChange);

        protected async override IAsyncEnumerable<Ticker> GetAsyncEnumerable()
        {
            var responseMessage = await this._client.GetAsync("https://www.paribu.com/ticker", HttpCompletionOption.ResponseHeadersRead);
            responseMessage.EnsureSuccessStatusCode();

            using var stream = await responseMessage.Content.ReadAsStreamAsync();

            var jsonObj = await JsonSerializer.DeserializeAsync<Dictionary<string, TickObj>>(stream);

            foreach (var tickObj in jsonObj)
            {
                var curs = tickObj.Key.Split('_');
                var baseCur = this._currencyFinder.Find(curs[0]);
                var quoteCurr = this._currencyFinder.Find(curs[1]);

                if (baseCur != null && quoteCurr != null)
                {
                    yield return new Ticker(new Pair(baseCur, quoteCurr))
                    {
                        LowestAsk = tickObj.Value.lowestAsk,
                        HighestBid = tickObj.Value.highestBid,
                        Change = tickObj.Value.change,
                        Last = tickObj.Value.last,
                        Volume = tickObj.Value.volume
                    };
                }
            }
        }
    }
}
