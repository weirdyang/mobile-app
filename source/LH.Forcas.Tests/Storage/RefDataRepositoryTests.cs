using System.Linq;
using LH.Forcas.Storage;
using NUnit.Framework;

namespace LH.Forcas.Tests.Storage
{
    using Forcas.Domain.RefData;

    [TestFixture]
    public class RefDataRepositoryTests
    {
        private IRefDataRepository refDataRepository;

        [SetUp]
        public void Setup()
        {
            this.refDataRepository = new RefDataRepository();
        }

        [TestFixture]
        public class LoadDataTests : RefDataRepositoryTests
        {
            [Test]
            public void LoadBanks()
            {
                var banks = this.refDataRepository.GetBanks();
                var rb = banks.SingleOrDefault(x => x.BankId == "RB");

                Assert.IsNotNull(rb);
                Assert.IsNotEmpty(rb.Name);
                Assert.IsNotEmpty(rb.CountryId);
                Assert.AreNotEqual(0, rb.BBAN);
                Assert.AreEqual(BankAuthorizationScheme.PerAccount, rb.AuthorizationScheme);
                Assert.IsTrue(rb.IsActive);
            }

            [Test]
            public void LoadCountries()
            {
                var countries = this.refDataRepository.GetCountries();
                var cz = countries.SingleOrDefault(x => x.CountryId == "CZ");

                Assert.IsNotNull(cz);
                Assert.IsNotEmpty(cz.DefaultCurrencyCode);
            }

            [Test]
            public void LoadCurrencies()
            {
                var currencies = this.refDataRepository.GetCurrencies();
                var cz = currencies.SingleOrDefault(x => x.CurrencyId == "CZK");

                Assert.IsNotNull(cz);
                Assert.IsNotEmpty(cz.DisplayName);
                Assert.IsNotEmpty(cz.DisplayFormat);
            }
        }
    }
}