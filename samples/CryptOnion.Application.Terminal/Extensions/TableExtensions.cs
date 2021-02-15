using Spectre.Console;

namespace CryptOnion.Application.Terminal.Extensions
{
    public static class TableExtensions
    {
        public static void AddTicker(this Table table, Ticker ticker)
        {
            var sign = (ticker.Change > 0) ? "[green]" : "[red]";
            var change = sign + ticker.Change.ToString() + "[/]";

            table.AddRow(ticker.Pair.ToString(), ticker.LowestAsk.ToString(), ticker.HighestBid.ToString(),
                            ticker.Last.ToString(), ticker.Volume.ToString(), change);
        }
    }
}
