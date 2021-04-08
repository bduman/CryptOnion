using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace CryptOnion.Currency
{
    public class Installer : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes
                    .FromAssemblyContaining<Installer>()
                    .Where(t => t.BaseType == typeof(CurrencyBase))
                    .WithServices(typeof(CurrencyBase))
                    .LifestyleSingleton()
            );
        }
    }
}
