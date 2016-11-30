using System.Linq;
using System.Threading.Tasks;
using LH.Forcas.Models.RefData;
using LH.Forcas.Storage;
using NUnit.Framework;
using SQLite;

namespace LH.Forcas.Tests.Storage
{
    [TestFixture]
    public class RefDataRepositoryTests
    {
        [SetUp]
        public void Setup()
        {
            this.dbManager = new TestsDbManager();
            this.dbManager.Initialize();

            this.dependencyService = new TestsDependencyService();
            this.dependencyService.Register((IDbManager)this.dbManager);

            this.refDataRepository = new RefDataRepository(this.dependencyService);
        }

        [TearDown]
        public void TearDown()
        {
            SQLiteAsyncConnection.ResetPool();
            this.dbManager.DeleteDatabase();
        }

        private TestsDbManager dbManager;
        private IRefDataRepository refDataRepository;
        private TestsDependencyService dependencyService;

        [Test]
        public async Task ShouldLoadSavedEntity()
        {
            var countries = await this.refDataRepository.GetRefDataAsync<Country>();

            Assert.IsNotNull(countries);
            Assert.IsFalse(countries.Any());

            await this.SaveCountryUpdate("CZE", 1);

            countries = await this.refDataRepository.GetRefDataAsync<Country>();
            var actualCountry = countries.Single();

            Assert.AreEqual("CZE", actualCountry.Code);
        }

        [Test]
        public async Task ShouldNotSaveUpdateIfSameVersionAlreadyExists()
        {
            await this.SaveCountryUpdate("CZE", 1);

            var countries = await this.refDataRepository.GetRefDataAsync<Country>();
            Assert.IsTrue(countries.Any());

            await this.SaveCountryUpdate("UK", 1);

            countries = await this.refDataRepository.GetRefDataAsync<Country>();

            Assert.IsTrue(countries.Any());
            Assert.AreEqual("CZE", countries.Single().Code);
        }

        private async Task SaveCountryUpdate(string countryCode, int version)
        {
            var country = new Country { Code = countryCode, DefaultCurrencyShortCode = "CZK" };
            var update = new RefDataUpdate<Country> { TypedData = new[] { country }, EntityType = typeof(Country), Version = version };

            await this.refDataRepository.SaveRefDataUpdates(new IRefDataUpdate[] { update });
        }
    }
}