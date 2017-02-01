namespace LH.Forcas.Tests.Views.Reusables.Converters
{
    using System;
    using System.Globalization;
    using Forcas.Views.Reusable.Converters;
    using NUnit.Framework;
    using Xamarin.Forms;

    [TestFixture]
    public class NumberColorIndicatorConverterTests
    {
        private NumberColorIndicatorConverter converter;

        [SetUp]
        public void Setup()
        {
            this.converter = new NumberColorIndicatorConverter();
            this.converter.PositiveColor = Color.Green;
            this.converter.NegativeColor = Color.Red;
        }

        [Test]
        public void ShouldConvertPositiveNumberToPositiveColor()
        {
            var color = this.converter.Convert(10m, typeof(Color), null, CultureInfo.CurrentCulture);

            Assert.AreEqual(Color.Green, color);
        }

        [Test]
        public void ShouldConvertNegativeNumberToNegativeColor()
        {
            var color = this.converter.Convert(-10m, typeof(Color), null, CultureInfo.CurrentCulture);

            Assert.AreEqual(Color.Red, color);
        }

        [Test]
        public void ShouldThrowOnInvalidValue()
        {
            Assert.Throws<ArgumentException>(() => this.converter.Convert("Hello", typeof(Color), null, CultureInfo.CurrentCulture));
        }
    }
}