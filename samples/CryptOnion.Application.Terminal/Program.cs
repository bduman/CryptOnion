using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Castle.Windsor;
using Castle.Windsor.Installer;
using CryptOnion.Application.Terminal.Filters;
using CryptOnion.Currency.Filter;
using CryptOnion.Exchange;
using CryptOnion.Observable;
using Spectre.Console;

namespace CryptOnion.Application.Terminal
{
    class Program
    {
        static async Task Main()
        {
            var container = new WindsorContainer();
            container.Install(
                FromAssembly.Named("CryptOnion"),
                FromAssembly.Named("CryptOnion.Exchange.Binance"),
                FromAssembly.Named("CryptOnion.Exchange.Paribu")
            );

            var terminal = new Terminal(AnsiConsole.Console);

            var exchange = container.Resolve<IExchange>("Paribu");
            var ticker = exchange.GetObservable<Ticker>();

            var tickerFilterHandler = container.Resolve<IFilterHandler<Ticker>>();
            tickerFilterHandler.Add(new HOTFilter());
            tickerFilterHandler.Add(new NonTLFilter());

            //ticker?.Window(ticker?.Where(x => x == byte.MinValue)).Subscribe(terminal);

            var sub = ticker.Window(TimeSpan.FromSeconds(3)).Subscribe(terminal);

            await terminal.Start();
        }
    }
}
