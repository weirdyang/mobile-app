using System;
using LH.Forcas.Banking.Providers;
using LH.Forcas.Domain.UserData.Authorization;
using NUnit.Framework;

namespace LH.Forcas.Tests.Banking.Providers
{
    [TestFixture]
    public class BankProviderInfoAttributeTests
    {
        public class WhenValidating : BankProviderInfoAttributeTests
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
}