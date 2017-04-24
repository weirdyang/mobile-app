using Windows.System;
using LH.Forcas.Events;
using LH.Forcas.UWP.Services;
using Prism.Events;
using Xamarin.Forms;

[assembly:Dependency(typeof(UwpDeviceService))]

namespace LH.Forcas.UWP.Services
{
    using System.Globalization;
    using Windows.Networking.Connectivity;
    using Forcas.Services;

    public class UwpDeviceService : IDeviceService
    {
        private readonly IEventAggregator eventAggregator;

        public UwpDeviceService(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;

            MemoryManager.AppMemoryUsageLimitChanging += this.HandleMemoryUsageLimitChanging;
        }

        public string CountryCode => RegionInfo.CurrentRegion.TwoLetterISORegionName;

        public bool IsNetworkAvailable
        {
            get
            {
                var profile = NetworkInformation.GetInternetConnectionProfile();

                if (profile == null)
                {
                    return false;
                }

                if (profile.IsWlanConnectionProfile || profile.IsWwanConnectionProfile)
                {
                    return true;
                }

                return false;
            }
        }

        private void HandleMemoryUsageLimitChanging(object sender, AppMemoryUsageLimitChangingEventArgs args)
        {
            if (args.NewLimit < MemoryManager.AppMemoryUsage)
            {
                this.eventAggregator.GetEvent<TrimMemoryRequestedEvent>().Publish(TrimMemorySeverity.ReleaseAll);
            }
            else if (args.NewLimit < args.OldLimit)
            {
                this.eventAggregator.GetEvent<TrimMemoryRequestedEvent>().Publish(TrimMemorySeverity.ReleaseLevel);
            }
        }
    }
}
