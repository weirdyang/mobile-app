using LH.Forcas.Models.RefData;
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
            this.dbManager.DeleteDatabase();
        }

        private TestsDbManager dbManager;

        [Test]
        public void ShouldCreateTables()
        {
            Assert.IsFalse(this.dbManager.DbFileExists);

            this.dbManager.Initialize();

            Assert.IsTrue(this.dbManager.DbFileExists);

            using (var connection = this.dbManager.GetSyncConnection())
            {
                Assert.IsNotNull(connection.GetTableInfo(typeof(Currency).Name));
                Assert.IsNotNull(connection.GetTableInfo(typeof(Bank).Name));
            }
        }
    }
}