using System;
using LH.Forcas.Storage;
using SQLite.Net;
using SQLite.Net.Interop;
using SQLite.Net.Platform.Win32;

namespace LH.Forcas.Tests.Storage
{
    public class TestsDbManager : DbManagerBase, IDisposable
    {
        private readonly SQLiteConnection connection;

        public TestsDbManager()
        {
            this.connection = this.GetSyncConnection();
        }

        protected override ISQLitePlatform GetPlatform()
        {
            return new SQLitePlatformWin32();
        }

        protected override string GetDbFilePath()
        {
            return "file::memory:?cache=shared";
        }

        public void Dispose()
        {
            this.connection.Dispose();
        }
    }
}