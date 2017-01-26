namespace LH.Forcas.ViewModels.Accounts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.RefData;
    using Domain.UserData;
    using FluentValidation;
    using LH.Forcas.Extensions;
    using LH.Forcas.Localization;
    using Prism.Navigation;
    using Prism.Services;
    using Services;

    public class AccountsDetailPageViewModel : DetailViewModelBase
    {
        private readonly IAccountingService accountingService;
        private readonly IRefDataService refDataService;
        private readonly IUserSettingsService userSettingsService;
        private readonly INavigationService navigationService;

        private string title;
        private string accountName;
        private Guid accountId;
        private IList<Bank> banks;
        private IList<Country> countries;
        private Bank selectedBank;
        private Country selectedCountry;
        private Type selectedAccountType;
        private IList<Currency> currencies;
        private Currency selectedCurrency;
        private bool canEditAccountType;

        public AccountsDetailPageViewModel(
            IAccountingService accountingService,
            IRefDataService refDataService,
            IUserSettingsService userSettingsService,
            INavigationService navigationService,
            IPageDialogService pageDialogService)
            : base(pageDialogService)
        {
            this.accountingService = accountingService;
            this.refDataService = refDataService;
            this.userSettingsService = userSettingsService;
            this.navigationService = navigationService;

            this.Validator = new AccountsDetailPageViewModelValidator();
        }

        public string Title
        {
            get { return this.title; }
            set { this.SetProperty(ref this.title, value); }
        }

        public bool CanEditAccountType
        {
            get { return this.canEditAccountType; }
            set { this.SetProperty(ref this.canEditAccountType, value); }
        }

        public string AccountName
        {
            get { return this.accountName; }
            set { this.SetProperty(ref this.accountName, value); }
        }

        public Bank SelectedBank
        {
            get { return this.selectedBank; }
            set { this.SetProperty(ref this.selectedBank, value); }
        }

        public IList<Bank> Banks
        {
            get { return this.banks; }
            private set { this.SetProperty(ref this.banks, value); }
        }

        public Country SelectedCountry
        {
            get { return this.selectedCountry; }
            set { this.SetProperty(ref this.selectedCountry, value); }
        }

        public IList<Country> Countries
        {
            get { return this.countries; }
            private set { this.SetProperty(ref this.countries, value); }
        }

        public Currency SelectedCurrency
        {
            get { return this.selectedCurrency; }
            set { this.SetProperty(ref this.selectedCurrency, value); }
        }

        public IList<Currency> Currencies
        {
            get { return this.currencies; }
            set { this.SetProperty(ref this.currencies, value); }
        }

        public Type SelectedAccountType
        {
            get { return this.selectedAccountType; }
            set { this.SetProperty(ref this.selectedAccountType, value); }
        }

        public IList<Type> AccountTypes { get; private set; }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            this.RunAsyncWithBusyIndicator(this.LoadData(parameters));
        }

        private async Task LoadData(NavigationParameters parameters)
        {
            try
            {
                this.Banks = await this.refDataService.GetBanks();
                this.Countries = await this.refDataService.GetCountriesAsync();
                this.Currencies = await this.refDataService.GetCurrencies();

                this.AccountTypes = new[]
                {
                    typeof(CashAccount),
                    typeof(BankAccount)
                    //typeof(LoanAccount)
                };

                if (parameters == null || parameters.Count == 0)
                {
                    this.CanEditAccountType = true;
                    this.Title = AppResources.AccountsDetailPage_Title_New;

                    var preferedCountryId = this.userSettingsService.CountryId;
                    this.SelectedCountry = this.Countries.SingleOrDefault(x => x.CountryId == preferedCountryId);
                }
                else
                {
                    this.accountId = (Guid) parameters[NavigationExtensions.AccountIdParameterName];
                    var account = this.accountingService.GetAccount(this.accountId);

                    this.Title = account.Name;
                    this.CanEditAccountType = false;

                    this.AccountName = account.Name;
                    this.SelectedAccountType = account.GetType();
                    this.SelectedCurrency = this.Currencies.SingleById(account.CurrencyId);

                    var bankAccount = account as BankAccount;
                    if (bankAccount != null)
                    {
                        this.SelectedBank = this.Banks.SingleById(bankAccount.BankId);
                        this.SelectedCountry = this.Countries.SingleById(this.SelectedBank.CountryId);
                    }
                }
            }
            catch (Exception)
            {
                // TODO: Log exception

                await this.DialogService.DisplayErrorAlert(AppResources.AccountsDetailPage_LoadError);
                await this.navigationService.GoBackAsync();
            }
        }

        protected override async Task Save()
        {
            try
            {
                var account = (Account)Activator.CreateInstance(this.SelectedAccountType);
                account.Name = this.AccountName;

                var cashAccount = account as CashAccount;
                if (cashAccount != null)
                {
                    cashAccount.CurrencyId = this.SelectedCurrency.CurrencyId;
                }

                var bankAccount = account as BankAccount;
                if (bankAccount != null)
                {
                    // bankAccount.AccountNumber
                    // bankAccount.CurrencyId
                    bankAccount.BankId = this.SelectedBank.BankId;
                }

                this.accountingService.SaveAccount(account);

                await base.Save();
                await this.navigationService.GoBackAsync();
            }
            catch (Exception)
            {
                // TODO: Log
                await this.DialogService.DisplayErrorAlert(AppResources.AccountsDetailPage_SaveError);
            }
        }

        public class AccountsDetailPageViewModelValidator : AbstractValidator<AccountsDetailPageViewModel>
        {
            public AccountsDetailPageViewModelValidator()
            {
                this.RuleFor(x => x.SelectedAccountType).NotNull();
                this.RuleFor(x => x.AccountName).NotEmpty();
                this.RuleFor(x => x.SelectedCurrency).NotNull();

                this.RuleFor(x => x.SelectedCountry).NotNull().When(x => x.SelectedAccountType == typeof(BankAccount));
                this.RuleFor(x => x.SelectedBank).NotNull().When(x => x.SelectedAccountType == typeof(BankAccount));
            }
        }
    }
}