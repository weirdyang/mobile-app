using LH.Forcas.Banking.Exceptions;
using LH.Forcas.Banking.Providers;
using LH.Forcas.Domain.UserData.Authorization;
using Moq;
using MvvmCross.Platform.IoC;
using NUnit.Framework;

namespace LH.Forcas.Tests.Banking.Providers
{
    [TestFixture]
    public class BankProviderFactoryTests
    {
        protected BankProviderFactory Factory;
        protected Mock<IMvxIoCProvider> IoCProviderMock;

        [SetUp]
        public void Setup()
        {
            this.IoCProviderMock = new Mock<IMvxIoCProvider>();
            this.Factory = new BankProviderFactory(this.IoCProviderMock.Object);
        }

        public class WhenCreatingAuthorization : BankProviderFactoryTests
        {
            [Test]
            public void ShouldCreateAuthorizationViaContainer()
            {
                this.Factory.Initialize(new[] { typeof(TestsBankProvider) });
                this.IoCProviderMock
                    .Setup(x => x.IoCConstruct(typeof(StaticTokenAuthorization)))
                    .Returns(new StaticTokenAuthorization());

                var auth = this.Factory.CreateAuthorization("RB");

                Assert.NotNull(auth);
                AssertEx.IsOfType<StaticTokenAuthorization>(auth);

                this.IoCProviderMock.VerifyAll();
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
            public void ShouldCreateProviderViaFactory()
            {
                this.Factory.Initialize(new[] { typeof(TestsBankProvider) });
                this.IoCProviderMock
                    .Setup(x => x.IoCConstruct(typeof(TestsBankProvider)))
                    .Returns(new TestsBankProvider());

                var provider = this.Factory.CreateProvider("RB");

                Assert.NotNull(provider);
                AssertEx.IsOfType<TestsBankProvider>(provider);

                this.IoCProviderMock.VerifyAll();
            }

            [Test]
            public void ShouldThrowWhenBankIdIsNotRecognized()
            {
                Assert.Throws<BankNotSupportedException>(() => this.Factory.CreateProvider("NotExistingId"));
            }
        }
    }
}