using System;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace CryptOnion.Exchange.Binance
{
    public class Installer : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component
                    .For<IExchange>()
                    .UsingFactoryMethod(BuildExchange)
                    .Named("Binance")
                    .LifestyleSingleton()
            );
        }

        private IExchange BuildExchange(IKernel kernel)
        {
            var realTimeTickerObservable = kernel.Resolve<IObservable<byte>>("Binance.RealTimeTicker");
            var tickerObservable = kernel.Resolve<IObservable<CryptOnion.Ticker>>("Binance.Ticker");

            var exchange = new Exchange();
            exchange.AddObservable(realTimeTickerObservable);
            exchange.AddObservable(tickerObservable);

            return exchange;
        }
    }
}
