using Flurl.Http.Testing;
using NUnit.Framework;

namespace LH.Forcas.Tests.Integration.Banks.Cz.Fio
{
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