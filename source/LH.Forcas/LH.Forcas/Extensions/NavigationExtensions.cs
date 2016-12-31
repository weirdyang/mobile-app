using System.Threading.Tasks;
using LH.Forcas.Integration;
using Prism.Navigation;

namespace LH.Forcas.Extensions
{
    using Xamarin.Forms;

    public static class NavigationExtensions
    {
        private static string _rootPageFormat;

        // Use this class to enfore passing strongly typed parameters and to avoid the navigation strings in the view model
        public const string ProviderAuthorizePage = "";

        public const string SyncFlowStateParameterName = "State";

        public static void InitializeNavigation()
        {
            if (Device.OS == TargetPlatform.Android)
            {
                _rootPageFormat = "http://forcas.com/RootSideMenuPage/{0}";
            }
            else
            {
                _rootPageFormat = "http://forcas.com/RootTabPage/{0}";
            }
        }

        public static async Task NavigateToDashboard(this INavigationService navigationService)
        {
            //await navigationService.NavigateAsync(string.Format(_rootPageFormat, "DashboardPage"));
            //await navigationService.NavigateAsync("RootSideMenuPage/DashboardNavigationPage/DashboardPage", animated:false);

            await navigationService.NavigateAsync("RootSideMenuPage/DashboardNavigationPage/DashboardPage", animated: false);
        }

        public static async Task NavigateToSyncProviderAuthorizationAsync(this INavigationService navigationService, IFileSyncProvider syncProvider)
        {
            var parameters = new NavigationParameters();
            parameters.Add("Provider", syncProvider);

            await navigationService.NavigateAsync(ProviderAuthorizePage, parameters);
        }
    }
}