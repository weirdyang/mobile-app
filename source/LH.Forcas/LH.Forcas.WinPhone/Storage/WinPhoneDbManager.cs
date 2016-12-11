using System.IO;
using Windows.Storage;
using LH.Forcas.Storage;
using LH.Forcas.WinPhone.Storage;
using SQLite.Net.Interop;
using SQLite.Net.Platform.WinRT;
using Xamarin.Forms;

[assembly:Dependency(typeof(WinPhoneDbManager))]

namespace LH.Forcas.WinPhone.Storage
{
    public class WinPhoneDbManager : DbManagerBase
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