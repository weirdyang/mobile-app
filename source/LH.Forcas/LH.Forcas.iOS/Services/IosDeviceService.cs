using System;
using CoreTelephony;
using Foundation;
using LH.Forcas.Events;
using LH.Forcas.iOS.Services;
using LH.Forcas.Services;
using UIKit;
using MvvmCross.Plugins.Messenger;

namespace LH.Forcas.iOS.Services
{
    public class IosDeviceService : IDeviceService, IDisposable
    {
        private static readonly Lazy<CTTelephonyNetworkInfo> TelNet = new Lazy<CTTelephonyNetworkInfo>(() => new CTTelephonyNetworkInfo());

        private NSObject memoryWarningListener;

        public void Initialize(IMvxMessenger messenger)
        {
            this.memoryWarningListener = UIApplication.Notifications.ObserveDidReceiveMemoryWarning((sender, args) => {
                messenger.Publish<TrimMemoryRequestedEvent>(new TrimMemoryRequestedEvent(this) { Severity = TrimMemorySeverity.ReleaseLevel });
            });
        }

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

        public void Dispose()
        {
            if (this.memoryWarningListener != null)
            {
                this.memoryWarningListener.Dispose();
                this.memoryWarningListener = null;
            }
        }
    }
}