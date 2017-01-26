namespace LH.Forcas.Tests.Views.Reusables.Converters
{
    using System.Globalization;
    using Forcas.Domain.UserData;
    using Forcas.Views.Reusable.Converters;
    using NUnit.Framework;

    [TestFixture]
    public class AccountTypeToResourceConverterTests
    {
        private AccountTypeToResourceConverter converter;

        [SetUp]
        public void Setup()
        {
            this.converter = new AccountTypeToResourceConverter();
        }

        [Test]
        public void ShouldConvertAllTypes()
        {
            Assert.IsNotEmpty(this.Convert<BankAccount>());
            Assert.IsNotEmpty(this.Convert<LoanAccount>());
            Assert.IsNotEmpty(this.Convert<CashAccount>());
        }

        [Test]
        public void ShouldConvertToCorrectResource()
        {
            Assert.AreEqual("Bank", this.Convert<BankAccount>());
        }

        private string Convert<T>()
        {
            return (string)this.converter.Convert(typeof(T), typeof(string), null, CultureInfo.CurrentCulture);
        }
    }
}