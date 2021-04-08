using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Castle.Windsor;
using Castle.Windsor.Installer;
using CryptOnion.Exchange;
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

            var exchange = container.Resolve<IExchange>("Binance");
            var ticker = exchange.GetObservable<byte>();

            System.Console.WriteLine("Main ThreadId: " + System.Threading.Thread.CurrentThread.ManagedThreadId);

            ticker?.Window(ticker?.Where(x => x == byte.MinValue)).Subscribe(terminal);

            //var sub = ticker?.Subscribe(terminal, (t) => t.Window(TimeSpan.FromSeconds(3)));

            await terminal.Start();
        }
    }
}
