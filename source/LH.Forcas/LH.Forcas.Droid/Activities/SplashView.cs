using Android.App;
using Android.OS;
using MvvmCross.Droid.Views;

namespace LH.Forcas.Droid.Activities
{
    using Android.Content.PM;

    [Activity(Label = "Forcas", MainLauncher = true, NoHistory = true, Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/Splash")]
    public class SplashView : MvxSplashScreenActivity
    {
        public SplashView()
            : base(Resource.Layout.Splash)
        {
        }
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
    }
}