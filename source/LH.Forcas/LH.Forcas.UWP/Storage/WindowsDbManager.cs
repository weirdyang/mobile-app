using System.IO;
using Windows.Storage;
using LH.Forcas.Storage;
using LH.Forcas.UWP.Storage;
using SQLite.Net.Interop;
using SQLite.Net.Platform.WinRT;
using Xamarin.Forms;

[assembly:Dependency(typeof(WindowsDbManager))]

namespace LH.Forcas.UWP.Storage
{
    public class WindowsDbManager : DbManagerBase
    {
        protected override ISQLitePlatform GetPlatform()
        {
            return new SQLitePlatformWinRT();
        }

        protected override string GetDbFilePath()
        {
            return Path.Combine(ApplicationData.Current.LocalFolder.Path, DbFileName);
        }
    }
}
