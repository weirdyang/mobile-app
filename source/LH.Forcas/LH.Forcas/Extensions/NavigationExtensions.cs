namespace LH.Forcas.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Prism.Navigation;
    using ViewModels.Accounts;
    using Views.Accounts;
    using Views.Dashboard;
    using Xamarin.Forms;

    public static class NavigationExtensions
    {
        private static string _rootPageName;

        // Use this class to enfore passing strongly typed parameters and to avoid the navigation strings in the view model
        public const string ProviderAuthorizePage = "";

        public const string FlowStateParameterName = "FlowState";
        public const string AccountIdParameterName = "AccountId";

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

        public static async Task NavigateToAccountDetail(this INavigationService navigationService, Guid accountId)
        {
            var uri = GetAbsoluteUri(nameof(AccountsNavigationPage), nameof(AccountsListPage), nameof(AccountsDetailPage));
            var parameters = CreateAccountDetailParameters(accountId);

            await navigationService.NavigateAsync(uri, parameters);
        }

        public static async Task StartAddAccountFlow(this INavigationService navigationService)
        {
            var flowState = new AddAccountFlowState();
            flowState.CancelNavigationAction = async () => await NavigateToAccounts(navigationService); // TBA - navigate to where the flow started

            var uri = GetAbsoluteUri(nameof(AccountsNavigationPage), nameof(AccountsAddTypeSelectionPage));

            var parameters = new NavigationParameters();
            parameters.Add(FlowStateParameterName, flowState);

            await navigationService.NavigateAsync(uri);
        }

        public static NavigationParameters CreateAccountDetailParameters(Guid accountId)
        {
            var parameters = new NavigationParameters();
            parameters.Add(AccountIdParameterName, accountId);

            return parameters;
        }

        public static TItem GetNavigationParameter<TItem>(this NavigationParameters parameters, string name)
        {
            object paramValue;
            if (!parameters.TryGetValue(name, out paramValue))
            {
                throw new ArgumentException($"The view has to be navigated with the '{name}' parameter.");
            }

            if (!(paramValue is TItem))
            {
                throw new ArgumentException($"The parameter '{name}' was expected with the type {typeof(TItem)}.");
            }

            return (TItem) paramValue;
        }

        private static Uri GetAbsoluteUri(string navPage, params string[] pages)
        {
            var pageUri = string.Join("/", pages);

            return new Uri($"app://forcas/{_rootPageName}/{navPage}/{pageUri}", UriKind.Absolute);
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