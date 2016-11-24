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

        private void Initialize()
        {
            // Check storage -> create DB, tables etc. if required
            // Is first run? 
            //    -> Change mainPage -> ask about the CloudSync
            //    -> Initialize Local store => Download config data (mandatory download!) - add a flag regarding failures or move crash reporting logic to local store
            //    -> Start the setup wizard
            // STORE ALL LOCAL DATA IN THE DATABASE - INCLUDING CONFIG DATA
        }
    }
}
