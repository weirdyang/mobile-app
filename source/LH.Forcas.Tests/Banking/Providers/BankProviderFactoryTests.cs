using LH.Forcas.Banking.Exceptions;
using LH.Forcas.Banking.Providers;
using LH.Forcas.Domain.UserData.Authorization;
using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;

namespace LH.Forcas.Tests.Banking.Providers
{
    [TestFixture]
    public class BankProviderFactoryTests
    {
        protected BankProviderFactory Factory;
        protected Mock<IUnityContainer> UnityContainerMock;

        [SetUp]
        public void Setup()
        {
            this.Factory = new BankProviderFactory(new UnityContainer());
        }

        public class WhenCreatingAuthorization : BankProviderFactoryTests
        {
            [Test]
            public void ShouldCreateAuthorizationViaContainer()
            {
                this.Factory.Initialize(new[] { typeof(TestsBankProvider) });

                var auth = this.Factory.CreateAuthorization("RB");

                Assert.IsNotNull(auth);
                Assert.IsInstanceOf<StaticTokenAuthorization>(auth);
            }

            [Test]
            public void ShouldThrowWhenBankIdIsNotRecognized()
            {
                Assert.Throws<BankNotSupportedException>(() => this.Factory.CreateAuthorization("NotExistingId"));
            }
        }

        public class WhenCreatingProvider : BankProviderFactoryTests
        {
            [Test]
            public void ShouldCreateProviderViaContainer()
            {
                // this.UnityContainerMock.Setup(x => x.Resolve(typeof(IBankProvider), "RB")).Returns(new TestsBankProvider());

                this.Factory.Initialize(new[] { typeof(TestsBankProvider) });
                var provider = this.Factory.CreateProvider("RB");

                Assert.IsNotNull(provider);
                Assert.IsInstanceOf<TestsBankProvider>(provider);
            }

            [Test]
            public void ShouldThrowWhenBankIdIsNotRecognized()
            {
                Assert.Throws<BankNotSupportedException>(() => this.Factory.CreateProvider("NotExistingId"));
            }
        }

        // TODO: Add edge cases - invalid id
    }
}