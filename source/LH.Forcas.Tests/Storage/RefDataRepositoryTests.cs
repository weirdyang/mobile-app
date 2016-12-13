using System.Collections.Generic;
using System.Linq;
using KellermanSoftware.CompareNetObjects;
using LH.Forcas.Domain.RefData;
using LH.Forcas.Storage;
using NUnit.Framework;
using TestStack.Dossier;

namespace LH.Forcas.Tests.Storage
{
    [TestFixture]
    public class RefDataRepositoryTests
    {
        private DbManager dbManager;
        private CompareLogic comparator;
        private IRefDataRepository refDataRepository;

        [SetUp]
        public void Setup()
        {
            this.dbManager = new TestsDbManager();
            this.comparator = new CompareLogic();

            this.refDataRepository = new RefDataRepository(this.dbManager);
        }

        [Test]
        public void ShouldLoadSavedBank()
        {
            this.comparator.Config.MembersToIgnore = new List<string> { "Id" };

            var banks = this.refDataRepository.GetBanks();

            Assert.IsNotNull(banks);
            Assert.IsFalse(banks.Any());

            var newBank = Builder<Bank>.CreateNew().Build();

            var update = new RefDataUpdate<Bank>();
            update.Version = 1;
            update.TypedData = new[] { newBank };

            this.refDataRepository.SaveRefDataUpdates(new IRefDataUpdate[] { update });

            banks = this.refDataRepository.GetBanks();
            var loadedBank = banks.Single();

            var comparisonResult = this.comparator.Compare(newBank, loadedBank);
            Assert.IsTrue(comparisonResult.AreEqual, comparisonResult.DifferencesString);
        }

        [Test]
        public void ShouldNotSaveUpdateIfSameVersionAlreadyExists()
        {
            this.SaveCountryUpdate("CZE", "CZK", 1);

            var countries = this.refDataRepository.GetCountries().ToArray();
            Assert.IsTrue(countries.Any());

            this.SaveCountryUpdate("UK", "GBP", 1);

            countries = this.refDataRepository.GetCountries().ToArray();

            Assert.IsTrue(countries.Any());
            Assert.AreEqual("CZE", countries.Single().Code);
        }

        [Test]
        public void ShouldSaveUpdateIfHasHigherVersion()
        {
            this.SaveCountryUpdate("CZE", "CZK", 1);

            var countries = this.refDataRepository.GetCountries().ToArray();
            Assert.IsTrue(countries.Any());

            this.SaveCountryUpdate("CZE", "GBP", 2);

            countries = this.refDataRepository.GetCountries().ToArray();

            Assert.IsTrue(countries.Any());
            Assert.AreEqual("CZE", countries.Single().Code);
            Assert.AreEqual("GBP", countries.Single().DefaultCurrencyCode);
        }

        private void SaveCountryUpdate(string countryCode, string defaultCurrencyCode, int version)
        {
            var country = new Country { Code = countryCode, DefaultCurrencyCode = defaultCurrencyCode };
            var update = new RefDataUpdate<Country> { TypedData = new[] { country }, Version = version };

            this.refDataRepository.SaveRefDataUpdates(new IRefDataUpdate[] { update });
        }
    }
}