using System;
using System.Linq;
using LH.Forcas.Domain.UserData;
using LH.Forcas.Storage;
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
                   .WithProperty(d => d.IsDeleted, false, true)
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
                   .WithProperty(d => d.IsDeleted, false, true)
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
                   .WithProperty(d => d.IsDeleted, false, true)
                   .Run();
            }
        }
    }
}