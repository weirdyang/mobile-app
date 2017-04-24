using System;
using CoreTelephony;
using Foundation;
using LH.Forcas.Events;
using LH.Forcas.iOS.Services;
using LH.Forcas.Services;
using Prism.Events;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(IosDeviceService))]

namespace LH.Forcas.iOS.Services
{
    public class IosDeviceService : IDeviceService, IDisposable
    {
        private static readonly Lazy<CTTelephonyNetworkInfo> TelNet = new Lazy<CTTelephonyNetworkInfo>(() => new CTTelephonyNetworkInfo());

        private readonly NSObject memoryWarningListener;

        public IosDeviceService(IEventAggregator eventAggregator)
        {
            this.memoryWarningListener = UIApplication.Notifications.ObserveDidReceiveMemoryWarning((sender, args) => {
                eventAggregator.GetEvent<TrimMemoryRequestedEvent>().Publish(TrimMemorySeverity.ReleaseLevel);
            });
        }

        public event EventHandler<TrimMemoryEventArgs> TrimMemoryRequested;

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
            this.memoryWarningListener.Dispose();
        }
    }
}