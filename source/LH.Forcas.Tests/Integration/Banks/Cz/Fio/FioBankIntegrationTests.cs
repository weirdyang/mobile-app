namespace LH.Forcas.Tests.Integration.Banks.Cze.Fio
{
    using Flurl.Http.Testing;
    using NUnit.Framework;

    [TestFixture]
    public class FioBankIntegrationTests
    {
        protected HttpTest FlurlTest;

        [SetUp]
        public void Setup()
        {
            this.FlurlTest = new HttpTest();
        }

        [TearDown]
        public void TearDown()
        {
            this.FlurlTest.Dispose();
        }

        public class FetchingAccounts : FioBankIntegrationTests
        {
            
        }

        public class FetchingTransactions : FioBankIntegrationTests
        {
            
        }
    }
}