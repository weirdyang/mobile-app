using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using LH.Forcas.Analytics;
using LH.Forcas.Domain.UserData;
using LH.Forcas.Extensions;
using LH.Forcas.Localization;
using LH.Forcas.Services;
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
            
            this.NavigateToAddAccountCommand = new AsyncDelegateCommand(this, this.navigationService.NavigateToAccountAdd);
            this.NavigateToAccountDetailCommand = new AsyncDelegateCommand<Account>(this, this.NavigateToAccountDetail);

            this.DeleteAccountCommand = new AsyncDelegateCommand<Account>(this, this.DeleteAccount);

            this.RefreshAccountsCommand = new AsyncDelegateCommand(this, this.RefreshAccounts);
        }

        public bool NoAccountsTextDisplayed => (this.AccountGroups == null || this.AccountGroups.Count == 0) && !this.IsBusy;

        public AsyncDelegateCommand NavigateToAddAccountCommand { get; }

        public AsyncDelegateCommand<Account> NavigateToAccountDetailCommand { get; }

        public AsyncDelegateCommand RefreshAccountsCommand { get; }

        public AsyncDelegateCommand<Account> DeleteAccountCommand { get; }

        public ObservableCollection<AccountsGroup> AccountGroups
        {
            get { return this.accountGroups; }
            private set { this.SetProperty(ref this.accountGroups, value); }
        }

        public override async Task OnNavigatingToAsync(NavigationParameters parameters)
        {
            await this.RefreshAccounts();
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(this.IsBusy) || args.PropertyName == nameof(this.AccountGroups))
            {
                this.RaisePropertyChanged(nameof(this.NoAccountsTextDisplayed));
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