using System.Linq;
using LH.Forcas.RefDataContract;
using LH.Forcas.Storage;
using NUnit.Framework;

namespace LH.Forcas.Tests.Storage
{
    [TestFixture]
    public class RefDataRepositoryTests
    {
        private IRefDataRepository refDataRepository;

        [SetUp]
        public void Setup()
        {
            this.refDataRepository = new RefDataRepository();
        }

        public class LoadDataTests : RefDataRepositoryTests
        {
            [Test]
            public void LoadBanks()
            {
                var banks = this.refDataRepository.GetBanks();
                var rb = banks.SingleOrDefault(x => x.BankId == "RB");

                Assert.NotNull(rb);
                Assert.IsNotEmpty(rb.Name);
                Assert.IsNotEmpty(rb.CountryId);
                Assert.AreNotEqual(0, rb.Bban);
                Assert.AreEqual(BankAuthorizationScope.PerAccount, rb.AuthorizationScope);
                Assert.True(rb.IsActive);
            }

            [Test]
            public void LoadCountries()
            {
                var countries = this.refDataRepository.GetCountries();
                var cz = countries.SingleOrDefault(x => x.CountryId == "CZ");

                Assert.NotNull(cz);
                Assert.IsNotEmpty(cz.DefaultCurrencyId);
            }

            [Test]
            public void LoadCurrencies()
            {
                var currencies = this.refDataRepository.GetCurrencies();
                var cz = currencies.SingleOrDefault(x => x.CurrencyId == "CZK");

                Assert.NotNull(cz);
                Assert.IsNotEmpty(cz.DisplayFormat);
            }
        }
    }
}