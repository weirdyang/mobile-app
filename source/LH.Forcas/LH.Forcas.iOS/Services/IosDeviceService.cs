using LH.Forcas.iOS.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(IosDeviceService))]

namespace LH.Forcas.iOS.Services
{
    using System;
    using CoreTelephony;
    using Forcas.Services;

    public class IosDeviceService : IDeviceService
    {
        private static readonly Lazy<CTTelephonyNetworkInfo> TelNet = new Lazy<CTTelephonyNetworkInfo>(() => new CTTelephonyNetworkInfo());

        public string CountryCode => TelNet.Value.SubscriberCellularProvider.IsoCountryCode;

        public bool IsNetworkAvailable
        {
            get
            {
                var status = Reachability.InternetConnectionStatus();

                return status == NetworkStatus.ReachableViaCarrierDataNetwork
                       || status == NetworkStatus.ReachableViaWiFiNetwork;
            }
        }
    }
}