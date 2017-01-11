using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LH.Forcas.Views.Accounts;
using LH.Forcas.Views.Dashboard;
using Prism.Navigation;

namespace LH.Forcas.Extensions
{
    using Xamarin.Forms;

    public static class NavigationExtensions
    {
        private static string _rootPageName;

        // Use this class to enfore passing strongly typed parameters and to avoid the navigation strings in the view model
        public const string ProviderAuthorizePage = "";

        public const string SyncFlowStateParameterName = "State";

        public static readonly IEnumerable<NavigationPage> RootLevelPages = new[]
        {
            new NavigationPage {DisplayName = "Dashboard", NavigateAction = NavigateToDashboard},
            new NavigationPage {DisplayName = "Accounts", NavigateAction = NavigateToAccounts}
        };

        public static void InitializeNavigation()
        {
            if (Device.OS == TargetPlatform.Android)
            {
                _rootPageName = "RootSideMenuPage";
            }
            else
            {
                _rootPageName = "RootTabPage";
            }
        }

        public static async Task NavigateToDashboard(this INavigationService navigationService)
        {
            await navigationService.NavigateAsync(GetAbsoluteUri(nameof(DashboardNavigationPage), nameof(DashboardPage)));
        }

        public static async Task NavigateToAccounts(this INavigationService navigationService)
        {
            var uri = GetAbsoluteUri(nameof(AccountsNavigationPage), nameof(AccountsListPage));
            await navigationService.NavigateAsync(uri);
        }

        private static Uri GetAbsoluteUri(string navPage, string page)
        {
            return new Uri($"app://forcas/{_rootPageName}/{navPage}/{page}", UriKind.Absolute);
        }
    }

    public delegate Task NavigateDelegate(INavigationService navigationService);

    public class NavigationPage
    {
        // TODO: Add data for icon conversion

        public string DisplayName { get; set; }

        public NavigateDelegate NavigateAction { get; set; }
    }
}