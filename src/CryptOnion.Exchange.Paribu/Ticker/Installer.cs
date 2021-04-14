using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CryptOnion.Observable;
using CryptOnion.Observable.Decorator;
using CryptOnion.Observable.Scheduled;

namespace CryptOnion.Exchange.Paribu.Ticker
{
    public class Installer : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component
                    .For<IDataProvider<IEnumerable<CryptOnion.Ticker>>>()
                    .ImplementedBy<DataProvider>()
                    .Named("Paribu.Ticker.DataProvider")
                    .LifestyleSingleton()
            );

            container.Register(
                Component
                    .For<IObservable<CryptOnion.Ticker>>()
                    .ImplementedBy<ShareDecorated<CryptOnion.Ticker>>()
                    .DependsOn(Dependency.OnComponent(typeof(IObservable<CryptOnion.Ticker>), "Paribu.Ticker.SubscribeOnDecorated"))
                    .Named("Paribu.Ticker"),
                Component
                    .For<IObservable<CryptOnion.Ticker>>()
                    .ImplementedBy<SubscribeOnDecorated<CryptOnion.Ticker>>()
                    .DependsOn(Dependency.OnValue(typeof(IScheduler), Scheduler.Default))
                    .DependsOn(Dependency.OnComponent(typeof(IObservable<CryptOnion.Ticker>), "Paribu.Ticker.ObserveOnDecorated"))
                    .Named("Paribu.Ticker.SubscribeOnDecorated"),
                Component
                    .For<IObservable<CryptOnion.Ticker>>()
                    .ImplementedBy<ObserveOnDecorated<CryptOnion.Ticker>>()
                    .DependsOn(Dependency.OnValue(typeof(IScheduler), Scheduler.Default))
                    .DependsOn(Dependency.OnComponent(typeof(IObservable<CryptOnion.Ticker>), "Paribu.Ticker.Filter"))
                    .Named("Paribu.Ticker.ObserveOnDecorated"),
                Component
                    .For<IObservable<CryptOnion.Ticker>>()
                    .ImplementedBy<FilterDecorated<CryptOnion.Ticker>>()
                    .DependsOn(Dependency.OnComponent(typeof(IObservable<CryptOnion.Ticker>), "Paribu.Ticker.Observable"))
                    .Named("Paribu.Ticker.Filter"),
                Component
                    .For<IObservable<CryptOnion.Ticker>>()
                    .ImplementedBy<Observable>()
                    .DependsOn(Dependency.OnComponent(typeof(IDataProvider<IEnumerable<CryptOnion.Ticker>>), "Paribu.Ticker.DataProvider"))
                    .DependsOn(Dependency.OnValue(typeof(IInternalDelay), new DefaultInternalDelay()))
                    .Named("Paribu.Ticker.Observable")
            );
        }
    }
}
