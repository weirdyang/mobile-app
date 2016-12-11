using System;
using System.Threading.Tasks;
using LH.Forcas.Integration;
using Prism.Navigation;

namespace LH.Forcas.Extensions
{
    public static class NavigationExtensions
    {
        // Use this to enfore passing strongly typed parameters and to avoid the navigation strings in the view model
        public const string ProviderAuthorizePage = "";

        public const string SyncFlowStateParameterName = "State";

        public static void NavigateToDashboard(this INavigationService navigationService)
        {
            navigationService.NavigateAsync("RootPage/Dashboard");
        }

        public static async Task NavigateToSyncProviderAuthorizationAsync(this INavigationService navigationService, IFileSyncProvider syncProvider)
        {
            var parameters = new NavigationParameters();
            parameters.Add("Provider", syncProvider);

            await navigationService.NavigateAsync(ProviderAuthorizePage, parameters);
        }
    }
}