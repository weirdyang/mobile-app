using System;
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
            this.Repository = new UserDataRepository(new TestsDbManager());
        }

        public class WhenHandlingSettings : UserDataRepositoryTests
        {
            [Test]
            public void SaveLoadTest()
            {
                new SaveLoadUtil<UserSettings>(
                    id => this.Repository.GetUserSettings(),
                    settings => this.Repository.SaveUserSettings(settings))
                    .WithProperty(d => d.DefaultCountryId, (d, val) => d.DefaultCountryId = val, "CZ", "UK")
                    .WithProperty(d => d.DefaultCurrencyId, (d, val) => d.DefaultCurrencyId = val, "CZK", "GBP")
                    .Run();
            }
        }
    }
}