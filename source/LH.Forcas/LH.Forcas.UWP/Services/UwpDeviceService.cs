using Windows.System;
using LH.Forcas.Events;
using LH.Forcas.UWP.Services;
using MvvmCross.Plugins.Messenger;

namespace LH.Forcas.UWP.Services
{
    using System.Globalization;
    using Windows.Networking.Connectivity;
    using Forcas.Services;

    public class UwpDeviceService : IDeviceService
    {
        private IMvxMessenger messenger;

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

        public void Initialize(IMvxMessenger _messenger)
        {
            this.messenger = _messenger;

            MemoryManager.AppMemoryUsageLimitChanging += this.HandleMemoryUsageLimitChanging;
        }

        private void HandleMemoryUsageLimitChanging(object sender, AppMemoryUsageLimitChangingEventArgs args)
        {
            if (args.NewLimit < MemoryManager.AppMemoryUsage)
            {
                this.messenger.Publish<TrimMemoryRequestedEvent>(TrimMemorySeverity.ReleaseAll);
            }
            else if (args.NewLimit < args.OldLimit)
            {
                this.messenger.Publish<TrimMemoryRequestedEvent>(TrimMemorySeverity.ReleaseLevel);
            }
        }
    }
}
