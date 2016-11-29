using System;
using System.IO;
using LH.Forcas.Storage;
using SQLite;

namespace LH.Forcas.Tests.Storage
{
    public class TestsDbManager : DbManagerBase
    {
        private readonly string filePath;

        public TestsDbManager()
        {
            var fileName = $"{Guid.NewGuid():N}.db3";
            this.filePath = Path.Combine(Path.GetTempPath(), fileName);

            Console.WriteLine("Using database file: {0}", this.filePath);
        }

        public bool DbFileExists => File.Exists(this.filePath);

        public override SQLiteConnection GetSyncConnection()
        {
            return new SQLiteConnection(this.filePath);
        }

        public override SQLiteAsyncConnection GetAsyncConnection()
        {
            return new SQLiteAsyncConnection(this.filePath);
        }

        public void DeleteDatabase()
        {
            File.Delete(this.filePath);
        }
    }
}