using LH.Forcas.Domain.RefData;
using NUnit.Framework;

namespace LH.Forcas.Tests.Storage
{
    [TestFixture]
    public class DbManagerBaseTests
    {
        [SetUp]
        public void Setup()
        {
            this.dbManager = new TestsDbManager();
        }

        [TearDown]
        public void TearDown()
        {
            this.dbManager.Dispose();
        }

        private TestsDbManager dbManager;

        [Test]
        public void ShouldCreateTables()
        {
            this.dbManager.Initialize();

            using (var connection = this.dbManager.GetSyncConnection())
            {
                Assert.IsNotNull(connection.GetTableInfo(typeof(Currency).Name));
                Assert.IsNotNull(connection.GetTableInfo(typeof(Bank).Name));
            }
        }
    }
}