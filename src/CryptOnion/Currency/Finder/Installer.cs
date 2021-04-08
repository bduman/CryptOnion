using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace CryptOnion.Currency.Finder
{
    public class Installer : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component
                    .For<ICurrencyFinder>()
                    .ImplementedBy<DefaultCurrencyFinder>()
                    .LifestyleSingleton()
            );
        }
    }
}
