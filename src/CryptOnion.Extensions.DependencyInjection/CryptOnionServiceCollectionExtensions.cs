using CryptOnion;
using CryptOnion.Currency;
using CryptOnion.Exchange;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CryptOnionServiceCollectionExtensions
    {
        public static IServiceCollection AddCryptOnion(this IServiceCollection services)
        {
            services.AddSingleton<ICurrencyFinder, DefaultCurrencyFinder>();

            return services;
        }

        public static IServiceCollection AddCryptOnionCurrency<T>(this IServiceCollection services) where T : AbstractCurrency
        {
            services.AddSingleton<AbstractCurrency, T>();

            return services;
        }

        public static IServiceCollection AddCryptOnionExchange<T>(this IServiceCollection services) where T : class, IExchange
        {
            services.AddSingleton<IExchange, T>();

            return services;
        }
    }
}
