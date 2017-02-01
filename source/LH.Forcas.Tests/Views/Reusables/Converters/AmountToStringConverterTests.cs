namespace LH.Forcas.Tests.Views.Reusables.Converters
{
    using System.Globalization;
    using Forcas.Domain.RefData;
    using Forcas.Services;
    using Forcas.Views.Reusable.Converters;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class AmountToStringConverterTests
    {
        private AmountToCurrencyStringConverter converter;

        private Currency currency;
        private Mock<IRefDataService> refDataServiceMock;

        [SetUp]
        public void Setup()
        {
            this.currency = new Currency {DisplayFormat = "€{0}"};

            this.refDataServiceMock = new Mock<IRefDataService>();
            this.refDataServiceMock.Setup(x => x.GetCurrency("EUR")).Returns(this.currency);

            this.converter = new AmountToCurrencyStringConverter();
        }

        [Test]
        public void ToEuroConversionTest()
        {
            var result = this.converter.Convert(10.58m, typeof(string), "EUR", CultureInfo.CurrentCulture);
            Assert.AreEqual("€10.58", result);

            this.refDataServiceMock.VerifyAll();
        }
    }
}