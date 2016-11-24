using LH.Forcas.Contract;
using LH.Forcas.Views;

namespace LH.Forcas
{
    public partial class App : IApp
    {
        public const string LastConfigDataSyncPropertyKey = "LastConfigDataSync";

        public App()
        {
            InitializeComponent();

            this.Constants = new AppConstants();

            MainPage = new MainPage();
        }

        public IAppConstants Constants { get; }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
