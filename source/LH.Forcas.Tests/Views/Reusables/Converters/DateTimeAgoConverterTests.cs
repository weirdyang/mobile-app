namespace LH.Forcas.Tests.Views.Reusables.Converters
{
    using System;
    using System.Globalization;
    using Forcas.Views.Reusable.Converters;
    using Localization;
    using NUnit.Framework;

    [TestFixture]
    public class DateTimeAgoConverterTests
    {
        protected DateTimeAgoConverter Converter;

        [SetUp]
        public void Setup()
        {
            this.Converter = new DateTimeAgoConverter();
        }

        [TestFixture]
        public class EdgeCasesTests : DateTimeAgoConverterTests
        {
            [Test]
            public void NullTest()
            {
                var result = this.Converter.Convert(null, typeof(string), null, CultureInfo.CurrentCulture);
                Assert.IsNull(result);
            }

            [Test]
            public void InvalidValueTest()
            {
                var result = this.Converter.Convert("DUMMY", typeof(string), null, CultureInfo.CurrentCulture);
                Assert.IsNull(result);
            }
        }

        [TestFixture]
        public class CalculationTest : DateTimeAgoConverterTests
        {
            [Test]
            public void NowTest()
            {
                var result = this.Converter.Convert(TimeSpan.FromSeconds(50), typeof(string), null, CultureInfo.CurrentCulture);
                Assert.AreEqual(AppResources.DateTimeAgo_Seconds, result);
            }

            [Test]
            public void MinuteTest()
            {
                var result = this.Converter.Convert(TimeSpan.FromSeconds(80), typeof(string), null, CultureInfo.CurrentCulture);
                Assert.AreEqual(AppResources.DateTimeAgo_Minute, result);
            }

            [Test]
            public void MinutesTest()
            {
                var result = (string)this.Converter.Convert(new TimeSpan(0, 4, 30), typeof(string), null, CultureInfo.CurrentCulture);
                AssertWithFormat(AppResources.DateTimeAgo_Minutes, result);
            }

            [Test]
            public void HourTest()
            {
                var result = this.Converter.Convert(new TimeSpan(1, 10, 55), typeof(string), null, CultureInfo.CurrentCulture);
                Assert.AreEqual(AppResources.DateTimeAgo_Hour, result);
            }

            [Test]
            public void HoursTest()
            {
                var result = (string)this.Converter.Convert(new TimeSpan(4, 4, 30), typeof(string), null, CultureInfo.CurrentCulture);
                AssertWithFormat(AppResources.DateTimeAgo_Hours, result);
            }

            [Test]
            public void DayTest()
            {
                var result = (string)this.Converter.Convert(new TimeSpan(27, 4, 30), typeof(string), null, CultureInfo.CurrentCulture);
                Assert.AreEqual(AppResources.DateTimeAgo_Day, result);
            }

            [Test]
            public void DaysTest()
            {
                var result = (string)this.Converter.Convert(new TimeSpan(50, 4, 30), typeof(string), null, CultureInfo.CurrentCulture);
                AssertWithFormat(AppResources.DateTimeAgo_Days, result);
            }

            [Test]
            public void WeekTest()
            {
                var result = (string)this.Converter.Convert(new TimeSpan(8, 20, 4, 30), typeof(string), null, CultureInfo.CurrentCulture);
                Assert.AreEqual(AppResources.DateTimeAgo_Week, result);
            }

            [Test]
            public void WeeksTest()
            {
                var result = (string)this.Converter.Convert(new TimeSpan(16, 21, 4, 30), typeof(string), null, CultureInfo.CurrentCulture);
                AssertWithFormat(AppResources.DateTimeAgo_Weeks, result);
            }

            [Test]
            public void MonthTest()
            {
                var result = (string)this.Converter.Convert(new TimeSpan(35, 21, 4, 30), typeof(string), null, CultureInfo.CurrentCulture);
                Assert.AreEqual(AppResources.DateTimeAgo_Month, result);
            }

            [Test]
            public void MonthsTest()
            {
                var result = (string)this.Converter.Convert(new TimeSpan(70, 21, 4, 30), typeof(string), null, CultureInfo.CurrentCulture);
                AssertWithFormat(AppResources.DateTimeAgo_Months, result);
            }

            [Test]
            public void YearTest()
            {
                var result = (string)this.Converter.Convert(new TimeSpan(390, 21, 4, 30), typeof(string), null, CultureInfo.CurrentCulture);
                Assert.AreEqual(AppResources.DateTimeAgo_Year, result);
            }

            [Test]
            public void YearsTest()
            {
                var result = (string)this.Converter.Convert(new TimeSpan(1200, 21, 4, 30), typeof(string), null, CultureInfo.CurrentCulture);
                AssertWithFormat(AppResources.DateTimeAgo_Years, result);
            }

            [Test]
            public void DateTimeTest()
            {
                var dateTime = DateTime.UtcNow.AddDays(-2);

                var result = (string)this.Converter.Convert(dateTime, typeof(string), null, CultureInfo.CurrentCulture);
                AssertWithFormat(AppResources.DateTimeAgo_Days, result);
            }

            private static void AssertWithFormat(string expectedFormat, object actual)
            {
                var formatEnd = expectedFormat.Replace("{0}", string.Empty);
                Assert.IsTrue(((string)actual).EndsWith(formatEnd));
            }
        }

        [TestFixture]
        public class CultureTests : DateTimeAgoConverterTests
        {
            
        }

        /*
         * Just now,
         * Minutes,
         * Hours,
         * Days,
         * Months,
         * Years
         * 
         * Refactor the number logic into a static extensions method with enum return value -> can be used for warning detection
         */
    }
}
