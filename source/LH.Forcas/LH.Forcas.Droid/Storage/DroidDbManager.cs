using System;
using System.IO;
using LH.Forcas.Droid.Storage;
using LH.Forcas.Storage;
using SQLite;
using Xamarin.Forms;

[assembly:Dependency(typeof(DroidDbManager))]

namespace LH.Forcas.Droid.Storage
{
    public class DroidDbManager : DbManagerBase
    {
        public override SQLiteAsyncConnection GetAsyncConnection()
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
            var dbFilePath =  Path.Combine(documentsPath, DbFileName);

            return new SQLiteAsyncConnection(dbFilePath);
        }
    }
}