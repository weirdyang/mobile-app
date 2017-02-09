
using LH.Forcas.Droid.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(DroidDeviceService))]

namespace LH.Forcas.Droid.Services
{
    using Android.App;
    using Android.Content;
    using Android.Net;
    using Android.Telephony;
    using Forcas.Services;

    public class DroidDeviceService : IDeviceService
    {
        public static TelephonyManager Manager => Application.Context.GetSystemService(Context.TelephonyService) as TelephonyManager;

        public static ConnectivityManager ConnectivityManager => Application.Context.GetSystemService(Context.ConnectivityService) as ConnectivityManager;

        public string CountryCode => Manager.SimCountryIso;

        public bool IsNetworkAvailable
        {
            get
            {
                var activeNetworkInfo = ConnectivityManager.ActiveNetworkInfo;
                return activeNetworkInfo != null && activeNetworkInfo.IsConnected;
            }
        }
    }
}