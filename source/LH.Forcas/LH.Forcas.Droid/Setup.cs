using MvvmCross.Droid.Platform;
using MvvmCross.Droid.Support.V7.AppCompat;

namespace LH.Forcas.Droid
{
    using Android.Content;
    using Forcas.Localization;
    using Forcas.Services;
    using Forcas.Storage;
    using Localization;
    using MvvmCross.Core.ViewModels;
    using MvvmCross.Platform;
    using Services;
    using Storage;

    public class Setup : MvxAppCompatSetup //MvxAndroidSetup
    {
        public Setup(Context applicationContext) 
            : base(applicationContext) { }

        protected override IMvxApplication CreateApp()
        {
            return new App();
        }

        protected override void InitializeFirstChance()
        {
            Mvx.ConstructAndRegisterSingleton<ILocale, DroidLocale>();
            Mvx.ConstructAndRegisterSingleton<IPathResolver, DroidPathResolver>();
            Mvx.ConstructAndRegisterSingleton<IDeviceService, DroidDeviceService>();

            base.InitializeFirstChance();
        }
    }
}