using System;
using System.IO;
using LH.Forcas.iOS.Storage;
using LH.Forcas.Storage;
using SQLite.Net.Interop;
using SQLite.Net.Platform.XamarinIOS;
using Xamarin.Forms;

[assembly:Dependency(typeof(IosDbManager))]

namespace LH.Forcas.iOS.Storage
{
    public class IosDbManager : DbManagerBase
    {
        protected override ISQLitePlatform GetPlatform()
        {
            return new SQLitePlatformIOS();
        }

        protected override string GetDbFilePath()
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
            var libraryPath = Path.Combine(documentsPath, "..", "Library"); // Library folder

            return Path.Combine(libraryPath, DbFileName);
        }
    }
}
