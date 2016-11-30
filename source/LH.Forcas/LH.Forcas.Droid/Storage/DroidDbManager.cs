using System;
using System.IO;
using LH.Forcas.Droid.Storage;
using LH.Forcas.Storage;
using SQLite.Net.Interop;
using SQLite.Net.Platform.XamarinAndroid;
using Xamarin.Forms;

[assembly:Dependency(typeof(DroidDbManager))]

namespace LH.Forcas.Droid.Storage
{
    public class DroidDbManager : DbManagerBase
    {
        protected override ISQLitePlatform GetPlatform()
        {
            return new SQLitePlatformAndroid();
        }

        protected override string GetDbFilePath()
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
            var dbFilePath = Path.Combine(documentsPath, DbFileName);

            return dbFilePath;
        }
    }
}