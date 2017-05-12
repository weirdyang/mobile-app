using System;
using System.Threading.Tasks;
using LH.Forcas.Views;
using LH.Forcas.Views.About;
using LH.Forcas.Views.Accounts;
using LH.Forcas.Views.Preferences;
using Prism.Navigation;

namespace LH.Forcas.Extensions
{
    public static class NavigationExtensions
    {
        // Use this class to enfore passing strongly typed parameters and to avoid the navigation strings in the view model
        public const string AccountIdParameterName = "AccountId";

        public static async Task NavigateToDashboard(this INavigationService navigationService)
        {
            await navigationService.NavigateAsync($"app://forcas/{nameof(RootNavigationPage)}/{nameof(RootTabPage)}/", null, false);
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
            throw new NotImplementedException();
        }

        public static async Task NavigateToCategoriesAdd(this INavigationService navigationService)
        {
            throw new NotImplementedException();
        }

        public static async Task NavigateToAbout(this INavigationService navigationService)
        {
            await navigationService.NavigateAsync(nameof(AboutPage), null, false);
        }

        public static async Task NavigateToPreferences(this INavigationService navigationService)
        {
            await navigationService.NavigateAsync(nameof(PreferencesPage), null, false);
        }

        public static async Task NavigateToLicense(this INavigationService navigationService)
        {
            await navigationService.NavigateAsync(nameof(LicensePage), null, false);
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

            return (TItem)paramValue;
        }

        private static Uri GetRelativeUri(string targetPage)
        {
            return new Uri(targetPage, UriKind.Relative);
        }
    }
}