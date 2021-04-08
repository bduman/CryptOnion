using System.Net.Http;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace CryptOnion
{
    public class Installer : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component
                    .For<HttpClient>()
                    .LifestyleTransient()
            );

            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel));
        }
    }
}
