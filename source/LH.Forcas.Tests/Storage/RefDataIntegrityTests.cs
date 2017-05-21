using System.Linq;
using LH.Forcas.Storage;
using NUnit.Framework;

namespace LH.Forcas.Tests.Storage
{
    [TestFixture]
    public class RefDataIntegrityTests
    {
        private IRefDataRepository refDataRepository;

        [Test]
        public void Setup()
        {
            this.refDataRepository = new RefDataRepository();
        }

        public class CountryCodesIntegrityChecks : RefDataIntegrityTests
        {
            [Test]
            public void BanksCountryCodesAreValid()
            {
                var countries = this.refDataRepository.GetCountries().ToArray();
                var banks = this.refDataRepository.GetBanks();

                foreach (var bank in banks)
                {
                    var country = countries.SingleOrDefault(x => x.CountryId == bank.CountryId);
                    Assert.NotNull(country);
                }
            }
        }

        public class CurrencyCodeIntegrityChecks : RefDataIntegrityTests
        {
            [Test]
            public void CountryDefaultCurrenciesAreValid()
            {
                var currencies = this.refDataRepository.GetCurrencies().ToArray();
                var countries = this.refDataRepository.GetCountries();

                foreach (var country in countries)
                {
                    var currency = currencies.SingleOrDefault(x => x.CurrencyId == country.DefaultCurrencyId);
                    Assert.NotNull(currency);
                }
            }
        }
    }
}