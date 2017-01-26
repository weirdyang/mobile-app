namespace LH.Forcas.Tests.Integration.Banks
{
    using System;
    using Forcas.Domain.UserData;
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
            this.Catalog.Initialize(this.GetType().Assembly);
            
            Assert.AreEqual(typeof(StaticTokenAuthorizationBase), this.Catalog.GetAuthorizationType("DummyId"));
        }

        [Test]
        public void ShouldCreateProviderViaContainer()
        {
            this.UnityContainerMock.Setup(x => x.Resolve(typeof(DummyBankProvider), null)).Returns(new DummyBankProvider());

            this.Catalog.Initialize(this.GetType().Assembly);
            this.Catalog.GetIntegrationProvider("DummyId");

            this.UnityContainerMock.VerifyAll();
        }
    }

    #region Helper Classes

    [BankProviderInfo(typeof(StaticTokenAuthorizationBase), "DummyId")]
    public class DummyBankProvider : IBankProvider
    {
        public void Initialize(BankAuthorizationBase authorizationBase) { }

        public Account[] FetchAccounts()
        {
            throw new NotImplementedException();
        }

        public Transaction[] FetchTransactions(Account account, DateTime lastDownloadTime)
        {
            throw new NotImplementedException();
        }
    }

#endregion
}