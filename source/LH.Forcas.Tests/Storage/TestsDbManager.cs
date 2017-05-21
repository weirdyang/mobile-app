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
            this.LiteRepository = new LiteRepository(this.stream);
        }

        public LiteRepository LiteRepository { get; }

        public void ApplyMigrations() { }

        public void Dispose()
        {
            this.LiteRepository.Dispose();
            this.stream.Dispose();
        }
    }
}