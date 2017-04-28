using Android.Content;
using Android.Content.Res;
using Android.Net;
using Android.Telephony;
using LH.Forcas.Droid.Services;
using LH.Forcas.Events;
using LH.Forcas.Services;
using Prism.Events;
using Xamarin.Forms;
using Application = Android.App.Application;

[assembly: Dependency(typeof(DroidDeviceService))]

namespace LH.Forcas.Droid.Services
{
    public class DroidDeviceService : IDeviceService
    {
        /// <summary>
        /// The static instance is a workaround for android object lifecycle where the CallbacksAdapter can be removed and re-instantiated at any time
        /// therefore it's not possible to subscribe to it's events or pass references
        /// </summary>
        private static DroidDeviceService deviceServiceInstance;

        public static TelephonyManager Manager => Application.Context.GetSystemService(Context.TelephonyService) as TelephonyManager;

        public static ConnectivityManager ConnectivityManager => Application.Context.GetSystemService(Context.ConnectivityService) as ConnectivityManager;

        private IEventAggregator eventAggregator;

        public void Initialize(IEventAggregator eventAggregator)
        {
            deviceServiceInstance = this;

            this.eventAggregator = eventAggregator;

            var adapter = new ComponentCallbacksAdapter();
            Application.Context.RegisterComponentCallbacks((IComponentCallbacks)adapter);
        }

        public string CountryCode => Manager.SimCountryIso;

        public bool IsNetworkAvailable
        {
            get
            {
                var activeNetworkInfo = ConnectivityManager.ActiveNetworkInfo;
                return activeNetworkInfo != null && activeNetworkInfo.IsConnected;
            }
        }

        #region ComponentCallbacksAdapter

        private class ComponentCallbacksAdapter : Java.Lang.Object, IComponentCallbacks2
        {
            public void OnConfigurationChanged(Configuration newConfig) { }

            public void OnLowMemory()
            {
                deviceServiceInstance.eventAggregator.GetEvent<TrimMemoryRequestedEvent>().Publish(TrimMemorySeverity.ReleaseAll);
            }

            public void OnTrimMemory(TrimMemory level)
            {
                var severity = this.TranslateToSeverity(level);
                deviceServiceInstance.eventAggregator.GetEvent<TrimMemoryRequestedEvent>().Publish(severity);
            }

            private TrimMemorySeverity TranslateToSeverity(TrimMemory level)
            {
                switch (level)
                {
                    case TrimMemory.Complete:
                    case TrimMemory.RunningCritical:
                        return TrimMemorySeverity.ReleaseAll;

                    default:
                        return TrimMemorySeverity.ReleaseLevel;
                }
            }
        }

        #endregion
    }
}