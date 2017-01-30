namespace LH.Forcas.Tests.Integration.Banks
{
    using Forcas.Domain.UserData.Authorization;
    using Forcas.Integration.Banks;
    using Microsoft.Practices.Unity;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class BankProviderCatalogTests
    {
        // TODO: Create tests that all banks in ref data have a provider in the catalog

        protected BankProviderCatalog Catalog;
        protected Mock<IUnityContainer> UnityContainerMock;

        [SetUp]
        public void Setup()
        {
            this.UnityContainerMock = new Mock<IUnityContainer>(MockBehavior.Strict);
            this.Catalog = new BankProviderCatalog(this.UnityContainerMock.Object);
        }

        [Test]
        public void ShouldLoadProviderTypes()
        {
            this.Catalog.Initialize(new [] { typeof(TestBankProvider) });
            
            Assert.AreEqual(typeof(StaticTokenAuthorization), this.Catalog.GetAuthorizationType("RB"));
        }

        [Test]
        public void ShouldCreateProviderViaContainer()
        {
            this.UnityContainerMock.Setup(x => x.Resolve(typeof(TestBankProvider), null)).Returns(new TestBankProvider());

            this.Catalog.Initialize(new[] { typeof(TestBankProvider) });
            this.Catalog.GetIntegrationProvider("RB");

            this.UnityContainerMock.VerifyAll();
        }
    }
}