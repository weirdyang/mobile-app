using System;
using System.IO;
using LH.Forcas.iOS.Storage;
using LH.Forcas.Storage;
using SQLite;
using Xamarin.Forms;

[assembly:Dependency(typeof(IosDbManager))]

namespace LH.Forcas.iOS.Storage
{
    public class IosDbManager : DbManagerBase
    {
        public override SQLiteAsyncConnection GetAsyncConnection()
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
            var libraryPath = Path.Combine(documentsPath, "..", "Library"); // Library folder
            var dbFilePath = Path.Combine(libraryPath, DbFileName);

            return new SQLiteAsyncConnection(dbFilePath);
        }
    }
}
