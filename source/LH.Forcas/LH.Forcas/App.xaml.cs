using System.Threading.Tasks;
using LH.Forcas.Contract;
using LH.Forcas.Views;
using Xamarin.Forms;

namespace LH.Forcas
{
    public partial class App : IApp
    {
        public const string LastRefDataSyncPropertyKey = "LastRefDataSync";

        public App()
        {
            this.InitializeComponent();

            this.Constants = new AppConstants();
            this.MainPage = new MainPage();
        }

        public IAppConstants Constants { get; }

        protected override void OnStart()
        {
            this.OnStartAsync().Wait();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        private async Task OnStartAsync()
        {
            var dbManager = DependencyService.Get<IDbManager>();
            await dbManager.Initialize();

            var userDataRepository = DependencyService.Get<IUserDataRepository>();
            var userPreferences = await userDataRepository.GetUserPreferencesAsync();

            if (userPreferences == null)
            {
                
            }

            // TODO: Is sync configured/disabled? If not -> wizard
            // TODO: The DB should also contain roaming settings - country/formats etc.

            // TODO: Has the user selected a country/formats etc.?
            // TODO: Does the user have any accounts? If not -> setup wizard

            // Check storage -> create DB, tables etc. if required
            // Is first run? 
            //    -> Change mainPage -> ask about the CloudSync
            //    -> Initialize Local store => Download config data (mandatory download!) - add a flag regarding failures or move crash reporting logic to local store
            //    -> Start the setup wizard
            // STORE ALL LOCAL DATA IN THE DATABASE - INCLUDING CONFIG DATA

            // TODO: Run sync in the behind (pass a cancellation token!)
            // TODO: Design background jobs - periodical sync (?), part of them will need to be implemented differently for each platform!
        }
    }
}
