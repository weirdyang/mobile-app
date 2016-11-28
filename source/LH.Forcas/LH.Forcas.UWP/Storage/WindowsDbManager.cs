using System.IO;
using Windows.Storage;
using LH.Forcas.Storage;
using LH.Forcas.UWP.Storage;
using SQLite;
using Xamarin.Forms;

[assembly:Dependency(typeof(WindowsDbManager))]

namespace LH.Forcas.UWP.Storage
{
    public class WindowsDbManager : DbManagerBase
    {
        public override SQLiteConnection GetSyncConnection()
        {
            return new SQLiteConnection(this.GetDbFilePath());
        }

        public override SQLiteAsyncConnection GetAsyncConnection()
        {
            return new SQLiteAsyncConnection(this.GetDbFilePath());
        }

        private string GetDbFilePath()
        {
            return Path.Combine(ApplicationData.Current.LocalFolder.Path, DbFileName);
        }
    }
}
