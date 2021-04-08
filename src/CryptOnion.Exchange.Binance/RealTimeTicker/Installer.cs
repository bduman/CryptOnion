using System;
using System.Net.WebSockets;
using System.Reactive.Concurrency;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CryptOnion.Observable;
using CryptOnion.Observable.Decorator;
using CryptOnion.Observable.WebSocket;

namespace CryptOnion.Exchange.Binance.RealTimeTicker
{
    public class Installer : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component
                    .For<Configuration>()
                    .Instance(new Configuration()
                    {
                        SocketUri = new Uri("wss://stream.binance.com:9443/ws/bnbbtc@ticker")
                    })
                    .Named("Binance.RealTimeTicker.Configuration")
                    .LifestyleSingleton()
            );

            container.Register(
                Component
                    .For<IDataProvider<ClientWebSocket>>()
                    .ImplementedBy<DefaultDataProvider>()
                    .DependsOn(Dependency.OnComponent(typeof(Configuration), "Binance.RealTimeTicker.Configuration"))
                    .Named("Binance.RealTimeTicker.DataProvider")
                    .LifestyleSingleton()
            );

            container.Register(
                Component
                    .For<IObservable<byte>>()
                    .ImplementedBy<ShareDecorated<byte>>()
                    .Named("Binance.RealTimeTicker"),
                Component
                    .For<IObservable<byte>>()
                    .ImplementedBy<SubscribeOnDecorated<byte>>()
                    .DependsOn(Dependency.OnValue(typeof(IScheduler), Scheduler.Default)),
                Component
                    .For<IObservable<byte>>()
                    .ImplementedBy<ObserveOnDecorated<byte>>()
                    .DependsOn(Dependency.OnValue(typeof(IScheduler), Scheduler.Default)),
                Component
                    .For<IObservable<byte>>()
                    .ImplementedBy<Observable>()
                    .DependsOn(Dependency.OnComponent(typeof(IDataProvider<ClientWebSocket>), "Binance.RealTimeTicker.DataProvider"))
            );
        }
    }
}
