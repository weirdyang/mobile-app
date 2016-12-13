using System;
using System.IO;
using LH.Forcas.Storage;
using LiteDB;

namespace LH.Forcas.Tests.Storage
{
    public class TestsDbManager : DbManager, IDisposable
    {
        private readonly MemoryStream stream;

        public TestsDbManager() : base(null)
        {
            this.stream = new MemoryStream();
        }

        public override LiteDatabase GetDatabase()
        {
            return new LiteDatabase(this.stream);
        }

        public void Dispose()
        {
            this.stream.Dispose();
        }
    }
}
