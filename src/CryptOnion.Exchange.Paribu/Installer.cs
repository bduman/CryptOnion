using System;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace CryptOnion.Exchange.Paribu
{
    public class Installer : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component
                    .For<IExchange>()
                    .UsingFactoryMethod(BuildExchange)
                    .Named("Paribu")
                    .LifestyleSingleton()
            );
        }

        private IExchange BuildExchange(IKernel kernel)
        {
            var tickerObservable = kernel.Resolve<IObservable<CryptOnion.Ticker>>("Paribu.Ticker");

            var exchange = new Exchange();
            exchange.AddObservable(tickerObservable);

            return exchange;
        }
    }
}
