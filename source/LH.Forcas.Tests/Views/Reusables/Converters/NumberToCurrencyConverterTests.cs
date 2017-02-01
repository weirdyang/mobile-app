namespace LH.Forcas.Tests.Views.Reusables.Converters
{
    using System.Globalization;
    using Forcas.Domain.RefData;
    using Forcas.Services;
    using Forcas.Views.Reusable.Converters;
    using Microsoft.Practices.Unity;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class NumberToCurrencyConverterTests
    {
        private NumberToCurrencyConverter converter;

        private Currency currency;
        private Mock<IUnityContainer> unityContainerMock = new Mock<IUnityContainer>();
        private Mock<IRefDataService> refDataServiceMock = new Mock<IRefDataService>();

        [SetUp]
        public void Setup()
        {
            this.currency = new Currency {DisplayFormat = "€{0}"};

            this.unityContainerMock.Setup(x => x.Resolve(typeof(IRefDataService), null)).Returns(this.refDataServiceMock.Object);
            this.refDataServiceMock.Setup(x => x.GetCurrency("EUR")).Returns(this.currency);

            App.GlobalContainer = this.unityContainerMock.Object;
            this.converter = new NumberToCurrencyConverter();
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