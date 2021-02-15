using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CryptOnion.Exchange;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

namespace CryptOnion.Application.Terminal
{
    class Program
    {
        static async Task Main()
        {
            var services = new ServiceCollection();
            services.AddHttpClient();
            services.AddCryptOnion();
            services.AddCryptOnionAllCurrencies();
            services.AddCryptOnionExchange<Exchange.Paribu.Exchange>();

            var provider = services.BuildServiceProvider();

            var terminal = new Terminal(AnsiConsole.Console);

            var exchanges = provider.GetServices<ExchangeBase>();
            var exchange = exchanges.First();

            var ticker = exchange.GetScheduledObservable<Ticker>();
            var sub = ticker?.Subscribe(terminal, (t) => t.Window(TimeSpan.FromSeconds(3)));

            await terminal.Start();
        }
    }
}
