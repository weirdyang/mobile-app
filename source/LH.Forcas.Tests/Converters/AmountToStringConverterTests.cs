using System.Globalization;
using LH.Forcas.Converters;
using LH.Forcas.Domain.UserData;
using LH.Forcas.RefDataContract;
using LH.Forcas.Services;
using Moq;
using NUnit.Framework;

namespace LH.Forcas.Tests.Converters
{
    [TestFixture]
    public class AmountToStringConverterTests
    {
        protected Currency Currency;
        protected Mock<IRefDataService> RefDataServiceMock;
        protected AmountToCurrencyStringConverter Converter;

        [SetUp]
        public void Setup()
        {
            this.Currency = new Currency { DisplayFormat = "€{0}" };

            this.RefDataServiceMock = new Mock<IRefDataService>();
            this.RefDataServiceMock.Setup(x => x.GetCurrency("EUR")).Returns(this.Currency);

            this.Converter = new AmountToCurrencyStringConverter();
            this.Converter.RefDataService = this.RefDataServiceMock.Object;
        }

        public class WhenConvertingTo : AmountToStringConverterTests
        {
            [Test]
            public void ToEuroConversionTest()
            {
                var result = this.Converter.Convert(new Amount(10.58m, "EUR"), typeof(string), null, new CultureInfo("en-US"));
                Assert.AreEqual("€10.58", result);

                this.RefDataServiceMock.VerifyAll();
            }
        }
    }
}