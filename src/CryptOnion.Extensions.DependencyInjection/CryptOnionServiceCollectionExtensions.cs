using CryptOnion;
using CryptOnion.Currency;
using CryptOnion.Exchange;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CryptOnionServiceCollectionExtensions
    {
        public static IServiceCollection AddCryptOnion(this IServiceCollection services)
        {
            services.AddSingleton<ICurrencyFinder, CurrencyFinder>((s) =>
            {
                var currencyFinder = new CurrencyFinder();
                var coins = s.GetServices<CurrencyBase>();

                foreach (var coin in coins)
                {
                    currencyFinder.Add(coin);
                }

                return currencyFinder;
            });

            return services;
        }

        public static IServiceCollection AddCryptOnionCurrency<T>(this IServiceCollection services) where T : CurrencyBase
        {
            services.AddSingleton<CurrencyBase, T>();

            return services;
        }

        public static IServiceCollection AddCryptOnionExchange<T>(this IServiceCollection services) where T : ExchangeBase
        {
            services.AddSingleton<ExchangeBase, T>();

            return services;
        }
    }
}
