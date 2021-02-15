using CryptOnion.Currency;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CryptOnionServiceCollectionExtensions
    {
        public static IServiceCollection AddCryptOnionAllCurrencies(this IServiceCollection services)
        {
            services.Scan(scan =>
            {
                scan.FromCallingAssembly()
                    .AddClasses(c => c.AssignableTo<CurrencyBase>().Where(t => t.IsSealed))
                    .As<CurrencyBase>()
                    .WithSingletonLifetime();
            });

            return services;
        }
    }
}
