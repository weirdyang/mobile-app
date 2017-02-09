namespace LH.Forcas.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Localization;
    using Prism.Navigation;
    using Views;
    using Views.Accounts;
    using Views.Categories;
    using Views.Dashboard;
    using Views.Settings;
    using Xamarin.Forms;

    public static class NavigationExtensions
    {
        private static string _rootPageName;

        // Use this class to enfore passing strongly typed parameters and to avoid the navigation strings in the view model
        public const string ProviderAuthorizePage = "";

        public const string FlowParameterName = "Flow";
        public const string AccountIdParameterName = "AccountId";

        public static readonly IEnumerable<NavigationPage> RootLevelPages = new[]
        {
            new NavigationPage { DisplayName = AppResources.Dashboard_Title, NavigateAction = NavigateToDashboard },
            new NavigationPage { DisplayName = AppResources.AccountsListPage_Title, NavigateAction = NavigateToAccounts },
            new NavigationPage { DisplayName = AppResources.CategoriesListPage_Title, NavigateAction = NavigateToCategories },
            new NavigationPage { DisplayName = AppResources.SettingsPage_Title, NavigateAction = NavigateToSettings }
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

        public static async Task NavigateToAccountAdd(this INavigationService navigationService)
        {
            await navigationService.NavigateAsync(nameof(AccountsDetailPage));
        }

        public static async Task NavigateToAccountDetail(this INavigationService navigationService, Guid accountId)
        {
            var parameters = CreateAccountDetailParameters(accountId);

            await navigationService.NavigateAsync(nameof(AccountsDetailPage), parameters);
        }

        public static async Task NavigateToCategories(this INavigationService navigationService)
        {
            var uri = GetAbsoluteUri(nameof(DashboardNavigationPage), nameof(CategoriesListPage));
            await navigationService.NavigateAsync(uri);
        }

        public static async Task NavigateToCategoriesAdd(this INavigationService navigationService)
        {
            throw new NotImplementedException();
        }

        public static async Task NavigateToSettings(this INavigationService navigationService)
        {
            await navigationService.NavigateAsync(GetRelativeUri(nameof(SettingsPage)));
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

        public static Uri GetAbsoluteUri(string navPage, params string[] pages)
        {
            var pageUri = string.Join("/", pages);

            return new Uri($"app://forcas/{_rootPageName}/{navPage}/{pageUri}", UriKind.Absolute);
        }

        public static string GetRelativeUri(string targetPage, string navPage = nameof(GenericNavigationPage))
        {
            return $"{navPage}/{targetPage}";
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