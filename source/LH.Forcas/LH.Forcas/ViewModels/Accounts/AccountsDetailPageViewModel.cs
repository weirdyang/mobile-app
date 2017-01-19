namespace LH.Forcas.ViewModels.Accounts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using LH.Forcas.Extensions;
    using LH.Forcas.Localization;
    using Prism.Navigation;
    using Services;

    public class AccountsDetailPageViewModel : ViewModelBase
    {
        private readonly IAccountingService accountingService;
        private readonly IRefDataService refDataService;
        private readonly IUserSettingsService userSettingsService;
        private readonly INavigationService navigationService;

        private string title;
        private Guid accountId;
        private IDictionary<string, string> banks;

        public AccountsDetailPageViewModel(
            IAccountingService accountingService, 
            IRefDataService refDataService,
            IUserSettingsService userSettingsService,
            INavigationService navigationService)
        {
            this.accountingService = accountingService;
            this.refDataService = refDataService;
            this.userSettingsService = userSettingsService;
            this.navigationService = navigationService;
        }

        public string Title
        {
            get { return this.title; }
            set { this.SetProperty(ref this.title, value); }
        }

        public IDictionary<string, string> Banks
        {
            get { return this.banks; }
            private set { this.SetProperty(ref this.banks, value); }
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            this.RunAsyncWithBusyIndicator(this.LoadData(parameters));
        }

        private async Task LoadData(NavigationParameters parameters)
        {
            await this.LoadBanks();

            if (parameters == null || parameters.Count == 0)
            {
                this.Title = AppResources.AccountsDetailPage_Title_New;
            }
            else
            {
                // TODO: Handle failure - id not found?

                this.accountId = (Guid)parameters[NavigationExtensions.AccountIdParameterName];
                var account = this.accountingService.GetAccount(this.accountId);

                this.Title = account.Name;
                // TODO: Load existing account
            }
        }

        private async Task LoadBanks()
        {
            var preferedCountry = this.userSettingsService.CountryCode;
            var banksByCountry = await this.refDataService.GetBanksByCountry(preferedCountry);

            this.Banks = banksByCountry.ToDictionary(
                x => x.BankId,
                x => x.Name
            );
        }
    }
}