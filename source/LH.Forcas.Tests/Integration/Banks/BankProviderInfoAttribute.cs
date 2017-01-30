namespace LH.Forcas.Tests.Integration.Banks
{
    using System;
    using Forcas.Domain.UserData.Authorization;
    using Forcas.Integration.Banks;
    using NUnit.Framework;

    [TestFixture]
    public class BankProviderInfoAttributeTests
    {
        [Test]
        public void ShouldThrowOnAbstractType()
        {
            var exception = Assert.Throws<ArgumentException>(() => new BankProviderInfoAttribute(typeof(BankAuthorizationBase), "DummyID"));

            Assert.IsTrue(exception.Message.Contains("Type"));
        }

        [Test]
        public void ShouldThrowOnNonDerivedType()
        {
            var exception = Assert.Throws<ArgumentException>(() => new BankProviderInfoAttribute(typeof(string), "DummyID"));

            Assert.IsTrue(exception.Message.Contains("Type"));
        }

        [Test]
        public void ShouldThrowIfNoIdIsProvided()
        {
            var exception = Assert.Throws<ArgumentException>(() => new BankProviderInfoAttribute(typeof(StaticTokenAuthorization)));

            Assert.IsTrue(exception.Message.Contains("Id"));
        }
    }
}