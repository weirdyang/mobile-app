using System;
using Android.App;
using Android.Content.PM;
using Android.OS;

namespace LH.Forcas.Droid
{
    [Activity(Label = "Forcas", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            AppDomain.CurrentDomain.UnhandledException += this.LogUnhandledException;

            Xamarin.Forms.Forms.Init(this, bundle);

            this.LoadApplication(new App());
        }

        private void LogUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}