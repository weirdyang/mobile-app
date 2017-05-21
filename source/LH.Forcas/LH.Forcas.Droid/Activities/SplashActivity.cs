using Android.App;
using Android.OS;
using MvvmCross.Droid.Views;

namespace LH.Forcas.Droid.Activities
{
    [Activity(Label = "Forcas", MainLauncher = true, NoHistory = true, Icon = "@drawable/icon")]
    public class SplashActivity : MvxSplashScreenActivity
    {
        public SplashActivity()
            : base(Resource.Layout.Splash)
        {
        }
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
        }
    }
}