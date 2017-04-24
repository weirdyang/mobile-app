using System;
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
        public static TelephonyManager Manager => Application.Context.GetSystemService(Context.TelephonyService) as TelephonyManager;

        public static ConnectivityManager ConnectivityManager => Application.Context.GetSystemService(Context.ConnectivityService) as ConnectivityManager;

        private readonly IEventAggregator eventAggregator;

        public DroidDeviceService(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;

            var adapter = new ComponentCallbacksAdapter(this);
            Application.Context.RegisterComponentCallbacks(adapter);
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

        private class ComponentCallbacksAdapter : IComponentCallbacks2
        {
            private readonly DroidDeviceService deviceService;

            public ComponentCallbacksAdapter(DroidDeviceService deviceService)
            {
                this.deviceService = deviceService;
            }

            public void Dispose() { }

            public IntPtr Handle { get; }

            public void OnConfigurationChanged(Configuration newConfig) { }

            public void OnLowMemory()
            {
                this.deviceService.eventAggregator.GetEvent<TrimMemoryRequestedEvent>().Publish(TrimMemorySeverity.ReleaseAll);
            }

            public void OnTrimMemory(TrimMemory level)
            {
                var severity = this.TranslateToSeverity(level);
                this.deviceService.eventAggregator.GetEvent<TrimMemoryRequestedEvent>().Publish(severity);
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