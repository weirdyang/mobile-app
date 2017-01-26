namespace LH.Forcas.Tests.Storage
{
    using System.Linq;
    using Forcas.Storage;
    using NUnit.Framework;

    [TestFixture]
    public class RefDataIntegrityTests
    {
        private IRefDataRepository refDataRepository;

        [SetUp]
        public void Setup()
        {
            this.refDataRepository = new RefDataRepository();
        }

        [TestFixture]
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
                    Assert.IsNotNull(country, $"Country with the code {bank.CountryId} could not be found.");
                }
            }
        }

        [TestFixture]
        public class CurrencyCodeIntegrityChecks : RefDataIntegrityTests
        {
            [Test]
            public void CountryDefaultCurrenciesAreValid()
            {
                var currencies = this.refDataRepository.GetCurrencies().ToArray();
                var countries = this.refDataRepository.GetCountries();

                foreach (var country in countries)
                {
                    var currency = currencies.SingleOrDefault(x => x.CurrencyId == country.DefaultCurrencyCode);
                    Assert.IsNotNull(currency, $"Currency with the code {country.DefaultCurrencyCode} could not be found");
                }
            }
        }
    }
}