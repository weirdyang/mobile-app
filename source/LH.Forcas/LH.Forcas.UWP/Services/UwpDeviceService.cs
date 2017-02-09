
using LH.Forcas.UWP.Services;
using Xamarin.Forms;

[assembly:Dependency(typeof(UwpDeviceService))]

namespace LH.Forcas.UWP.Services
{
    using System.Globalization;
    using Windows.Networking.Connectivity;
    using Forcas.Services;
    public class UwpDeviceService : IDeviceService
    {
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
    }
}
