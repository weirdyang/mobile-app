using System;
using System.Globalization;
using System.Linq;
using LH.Forcas.Analytics;
using LH.Forcas.Domain.UserData;
using LH.Forcas.RefDataContract.Parsing;
using LH.Forcas.Storage;
using LH.Forcas.Sync.RefData;
using Microsoft.Practices.Unity;
using NUnit.Framework;

namespace LH.Forcas.Tests.Storage
{
    [TestFixture]
    public class UserDataRepositoryTests
    {
        protected UserDataRepository Repository;

        [SetUp]
        public void Setup()
        {
            var dbManager = new TestsDbManager();
            dbManager.ApplyMigrations();

            this.Repository = new UserDataRepository(dbManager);
        }

        [Test]
        public void Dummy()
        {
            var container = new UnityContainer();

            container.RegisterType<IAnalyticsReporter, AnalyticsReporter>(new ContainerControlledLifetimeManager());

            container.RegisterType<IGitHubClientFactory, GitHubClientFactory>(new ContainerControlledLifetimeManager());
            container.RegisterType<IRefDataDownloader, RefDataDownloader>(new ContainerControlledLifetimeManager());
            container.RegisterType<IRefDataUpdateParser, RefDataUpdateParser>(new ContainerControlledLifetimeManager());

            container.RegisterType<IDbManager, DbManager>(new ContainerControlledLifetimeManager());
        }

        public class WhenHandlingSettings : UserDataRepositoryTests
        {
            [Test]
            public void ShouldSaveLoadUserSettings()
            {
                new SaveLoadUtil<UserSettings, string>(
                    id => this.Repository.GetUserSettings(),
                    settings => this.Repository.SaveUserSettings(settings),
                    null)
                    .WithProperty(d => d.DefaultCountryId, "CZ", "UK")
                    .WithProperty(d => d.DefaultCurrencyId, "CZK", "GBP")
                    .Run();
            }
        }

        public class WhenHandlingAccounts : UserDataRepositoryTests
        {
            [Test]
            public void ShouldSaveLoadCashAccount()
            {
                new SaveLoadUtil<CashAccount, Guid>(
                   () => this.Repository.GetAccounts().OfType<CashAccount>(),
                   account => this.Repository.SaveAccount(account),
                   Guid.NewGuid())
                   .WithProperty(d => d.CurrencyId, "CZK", "GBP")
                   .WithProperty(d => d.Name, "Name1", "Name2")
                   .WithProperty(d => d.CurrentBalance, new Amount(10, "CZK"), new Amount(20, "CZK"))
                   .WithProperty(d => d.LastSyncUtcTime, new DateTime(2017, 1, 1), new DateTime(2018, 1, 1))
                   .Run();
            }

            [Test]
            public void ShouldSaveLoadCheckingAccount()
            {
                new SaveLoadUtil<CheckingAccount, Guid>(
                   () => this.Repository.GetAccounts().OfType<CheckingAccount>(),
                   account => this.Repository.SaveAccount(account),
                   Guid.NewGuid())
                   .WithProperty(d => d.BankId, "B1", "B2")
                   .WithProperty(d => d.AccountNumber, AccountNumber.FromCzLocal("123456/5500"), AccountNumber.FromCzLocal("999999/5500"))
                   .WithProperty(d => d.CurrencyId, "CZK", "GBP")
                   .WithProperty(d => d.Name, "Name1", "Name2")
                   .WithProperty(d => d.CurrentBalance, new Amount(10, "CZK"), new Amount(20, "CZK"))
                   .WithProperty(d => d.LastSyncUtcTime, new DateTime(2017, 1, 1), new DateTime(2018, 1, 1))
                   .Run();
            }

            [Test]
            public void ShouldSaveLoadCreditCard()
            {
                new SaveLoadUtil<CreditCardAccount, Guid>(
                   () => this.Repository.GetAccounts().OfType<CreditCardAccount>(),
                   account => this.Repository.SaveAccount(account),
                   Guid.NewGuid())
                   .WithProperty(d => d.CardNumber, "123456", "99999")
                   .WithProperty(d => d.CurrencyId, "CZK", "GBP")
                   .WithProperty(d => d.Name, "Name1", "Name2")
                   .WithProperty(d => d.CurrentBalance, new Amount(10, "CZK"), new Amount(20, "CZK"))
                   .WithProperty(d => d.LastSyncUtcTime, new DateTime(2017, 1, 1), new DateTime(2018, 1, 1))
                   .Run();
            }
        }
    }
}