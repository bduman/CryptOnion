using System;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptOnion.Application.Terminal.Extensions;
using Spectre.Console;

namespace CryptOnion.Application.Terminal
{
    public class Terminal : IObserver<IObservable<Ticker>>, IObserver<IObservable<byte>>
    {
        private readonly IAnsiConsole _terminal;

        public Terminal(IAnsiConsole terminal)
        {
            this._terminal = terminal;
        }

        private Table CreateTable()
        {
            var table = new Table();

            table.AddColumn("Pair");
            table.AddColumn("Lowest Ask");
            table.AddColumn("Highest Bid");
            table.AddColumn("Last");
            table.AddColumn("Volume");
            table.AddColumn("Change");

            return table;
        }

        public async Task Start()
        {
            await this._terminal.Status()
                .Spinner(Spinner.Known.Moon)
                .StartAsync("Working..", async ctx =>
                {
                    ctx.Refresh();
                    await new TaskCompletionSource<object>().Task;
                });
        }

        public void OnNext(IObservable<Ticker> value)
        {
            value.Aggregate(this.CreateTable(), (table, ticker) =>
            {
                table.AddTicker(ticker);

                return table;
            })
            .Where(t => t.Rows.Count > 0)
            .Subscribe(t =>
            {
                this._terminal.Clear(true);
                this._terminal.Render(t);
                this._terminal.MarkupLine("Last update: " + DateTime.Now.ToLongTimeString());
            });
        }

        public void OnCompleted()
        {
            System.Console.WriteLine("completed");
        }

        public void OnError(Exception error)
        {
            System.Console.WriteLine("error {0}", error.Message);
        }

        public void OnNext(IObservable<byte> value)
        {
            value.SkipLast(1).Aggregate(new StringBuilder(), (s, b) =>
            {
                s.Append((char)b);
                return s;
            })
            .Select(sb => sb.ToString())
            .Subscribe(System.Console.WriteLine);
        }
    }
}
