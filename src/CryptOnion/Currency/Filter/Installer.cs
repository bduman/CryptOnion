using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace CryptOnion.Currency.Filter
{
    public class Installer : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component
                    .For(typeof(IFilterHandler<>))
                    .ImplementedBy(typeof(GenericFilterHandler<>))
                    .LifestyleSingleton()
            );
        }
    }
}
