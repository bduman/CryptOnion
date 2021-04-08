using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CryptOnion.Observable;
using CryptOnion.Observable.Decorator;
using CryptOnion.Observable.Scheduled;

namespace CryptOnion.Exchange.Binance.Ticker
{
    public class Installer : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component
                    .For<IDataProvider<IEnumerable<CryptOnion.Ticker>>>()
                    .ImplementedBy<DataProvider>()
                    .Named("Binance.Ticker.DataProvider")
                    .LifestyleSingleton()
            );

            container.Register(
                Component
                    .For<IObservable<CryptOnion.Ticker>>()
                    .ImplementedBy<ShareDecorated<CryptOnion.Ticker>>()
                    .Named("Binance.Ticker"),
                Component
                    .For<IObservable<CryptOnion.Ticker>>()
                    .ImplementedBy<SubscribeOnDecorated<CryptOnion.Ticker>>()
                    .DependsOn(Dependency.OnValue(typeof(IScheduler), Scheduler.Default)),
                Component
                    .For<IObservable<CryptOnion.Ticker>>()
                    .ImplementedBy<ObserveOnDecorated<CryptOnion.Ticker>>()
                    .DependsOn(Dependency.OnValue(typeof(IScheduler), Scheduler.Default)),
                Component
                    .For<IObservable<CryptOnion.Ticker>>()
                    .ImplementedBy<Observable>()
                    .DependsOn(Dependency.OnComponent(typeof(IDataProvider<IEnumerable<CryptOnion.Ticker>>), "Binance.Ticker.DataProvider"))
                    .DependsOn(Dependency.OnValue(typeof(IInternalDelay), new DefaultInternalDelay()))
            );
        }
    }
}
