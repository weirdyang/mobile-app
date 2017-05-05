using System.IO;
using LH.Forcas.Storage;
using LiteDB;

namespace LH.Forcas.Tests.Storage
{
    public class TestsDbManager : IDbManager
    {
        private readonly MemoryStream stream;

        public TestsDbManager()
        {
            DbManager.RegisterBsonMappings();

            this.stream = new MemoryStream();
            this.Database = new LiteDatabase(this.stream);
        }

        public LiteDatabase Database { get; }

        public void ApplyMigrations() { }

        public void Dispose()
        {
            this.Database.Dispose();
            this.stream.Dispose();
        }
    }
}