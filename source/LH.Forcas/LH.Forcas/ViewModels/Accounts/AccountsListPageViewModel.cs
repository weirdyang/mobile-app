using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using LH.Forcas.Analytics;
using LH.Forcas.Domain.UserData;
using LH.Forcas.Extensions;
using LH.Forcas.Localization;
using LH.Forcas.Services;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;

namespace LH.Forcas.ViewModels.Accounts
{
    public class AccountsListPageViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IPageDialogService dialogService;
        private readonly IAnalyticsReporter analyticsReporter;
        private readonly IAccountingService accountingService;
        private readonly Type[] accountTypeOrder = { typeof(CashAccount), typeof(CheckingAccount), typeof(CreditCardAccount), typeof(SavingsAccount), typeof(LoanAccount), typeof(InvestmentAccount) };

        private ObservableCollection<AccountsGroup> accountGroups;

        public AccountsListPageViewModel(
            IAccountingService accountingService,
            INavigationService navigationService,
            IPageDialogService dialogService,
            IAnalyticsReporter analyticsReporter)
        {
            this.accountingService = accountingService;
            this.navigationService = navigationService;
            this.dialogService = dialogService;
            this.analyticsReporter = analyticsReporter;

            this.NavigateToAddAccountCommand = DelegateCommand.FromAsyncHandler(this.navigationService.NavigateToAccountAdd);
            this.NavigateToAccountDetailCommand = DelegateCommand<Account>.FromAsyncHandler(this.NavigateToAccountDetail);

            this.DeleteAccountCommand = DelegateCommand<Account>.FromAsyncHandler(this.DeleteAccount);

            this.RefreshAccountsCommand = DelegateCommand.FromAsyncHandler(this.RefreshAccounts);
        }

        public bool NoAccountsTextDisplayed => (this.AccountGroups == null || this.AccountGroups.Count == 0) && !this.IsBusy;

        public DelegateCommand NavigateToAddAccountCommand { get; private set; }

        public DelegateCommand<Account> NavigateToAccountDetailCommand { get; private set; }

        public DelegateCommand RefreshAccountsCommand { get; private set; }

        public DelegateCommand<Account> DeleteAccountCommand { get; private set; }

        public ObservableCollection<AccountsGroup> AccountGroups
        {
            get { return this.accountGroups; }
            private set { this.SetProperty(ref this.accountGroups, value); }
        }

        public override async void OnNavigatedTo(NavigationParameters parameters)
        {
            await this.RefreshAccounts();
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            // ReSharper disable once ExplicitCallerInfoArgument
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(this.IsBusy) || propertyName == nameof(this.AccountGroups))
            {
                this.OnPropertyChanged(nameof(this.NoAccountsTextDisplayed));
            }
        }

        private async Task RefreshAccounts()
        {
            await this.RunAsyncWithBusyIndicator(() =>
                                                 {
                                                     var accounts = this.accountingService.GetAccounts();

                                                     if (accounts != null)
                                                     {
                                                         var filtered = accounts.Where(x => !x.IsDeleted);
                                                         this.AccountGroups = this.GroupAccounts(filtered);
                                                     }
                                                 });
        }

        private async Task NavigateToAccountDetail(Account account)
        {
            if (account == null)
            {
                return;
            }

            await this.navigationService.NavigateToAccountDetail(account.Id);
        }

        private async Task DeleteAccount(Account account)
        {
            if (account == null)
            {
                return;
            }

            var confirmMsg = string.Format(AppResources.AccountsListPage_DeleteAccountConfirmMsgFormat, account.Name);

            var confirmed = await this.dialogService.DisplayAlertAsync(
                          AppResources.AccountsListPage_DeleteAccountConfirmTitle,
                          confirmMsg,
                          AppResources.ConfirmDialog_Yes,
                          AppResources.ConfirmDialog_No);

            if (!confirmed)
            {
                return;
            }

            try
            {
                this.accountingService.DeleteAccount(account.Id);
                this.AccountGroups.Single(x => x.AccountType == account.GetType()).Remove(account);
            }
            catch (Exception ex)
            {
                this.analyticsReporter.ReportHandledException(ex);
                await this.dialogService.DisplayErrorAlert(AppResources.AccountsListPage_DeleteAccountError);
            }
            finally
            {
                await this.RefreshAccounts();
            }
        }

        private ObservableCollection<AccountsGroup> GroupAccounts(IEnumerable<Account> accounts)
        {
            var groups = accounts.GroupBy(account => account.GetType())
                           .Select(group => new AccountsGroup(group.Key, group.OrderBy(acc => acc.Name)))
                           .OrderBy(group => Array.IndexOf(this.accountTypeOrder, group.AccountType));

            return new ObservableCollection<AccountsGroup>(groups);
        }

        public class AccountsGroup : ObservableCollection<Account>
        {
            public AccountsGroup(Type accountType, IEnumerable<Account> accounts)
                : base(accounts)
            {
                this.AccountType = accountType;
            }

            public Type AccountType { get; }
        }
    }
}