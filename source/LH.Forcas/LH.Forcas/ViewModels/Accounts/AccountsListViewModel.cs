using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Chance.MvvmCross.Plugins.UserInteraction;
using LH.Forcas.Analytics;
using LH.Forcas.Domain.UserData;
using LH.Forcas.Localization;
using LH.Forcas.Services;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace LH.Forcas.ViewModels.Accounts
{
    public class AccountsListViewModel : MvxViewModel
    {
        private readonly IUserInteraction userInteraction;
        private readonly IAnalyticsReporter analyticsReporter;
        private readonly IMvxNavigationService navigationService;
        private readonly IAccountingService accountingService;
        private readonly Type[] accountTypeOrder = { typeof(CashAccount), typeof(CheckingAccount), typeof(CreditCardAccount), typeof(SavingsAccount), typeof(LoanAccount), typeof(InvestmentAccount) };

        private ObservableCollection<AccountsGroup> accountGroups;

        public AccountsListViewModel(
            IMvxNavigationService navigationService,
            IAccountingService accountingService,
            IUserInteraction userInteraction,
            IAnalyticsReporter analyticsReporter)
        {
            this.navigationService = navigationService;
            this.accountingService = accountingService;
            this.userInteraction = userInteraction;
            this.analyticsReporter = analyticsReporter;

            this.ActivityIndicatorState = new ActivityIndicatorState();

            this.RefreshAccountsCommand = new MvxAsyncCommand(this.ActivityIndicatorState.WrapWithIndicator((Action)this.RefreshAccounts));
            this.NavigateToAddAccountCommand = new MvxAsyncCommand(this.ActivityIndicatorState.WrapWithIndicator(navigationService.Navigate<AccountsDetailPageViewModel>));

            this.NavigateToAccountDetailCommand = new MvxAsyncCommand<Account>(this.ActivityIndicatorState.WrapWithIndicator<Account>(this.NavigateToAccountDetail));
            this.DeleteAccountCommand = new MvxAsyncCommand<Account>(this.ActivityIndicatorState.WrapWithIndicator<Account>(this.DeleteAccount));
        }

        public ActivityIndicatorState ActivityIndicatorState { get; set; }

        public bool NoAccountsTextDisplayed => (this.AccountGroups == null || this.AccountGroups.Count == 0);

        public MvxAsyncCommand NavigateToAddAccountCommand { get; }

        public MvxAsyncCommand<Account> NavigateToAccountDetailCommand { get; }

        public MvxAsyncCommand RefreshAccountsCommand { get; }

        public MvxAsyncCommand<Account> DeleteAccountCommand { get; }

        public ObservableCollection<AccountsGroup> AccountGroups
        {
            get => this.accountGroups;
            private set
            {
                this.SetProperty(ref this.accountGroups, value);
                
                // ReSharper disable once ExplicitCallerInfoArgument
                this.RaisePropertyChanged(nameof(this.NoAccountsTextDisplayed));
            }
        }

        public override void Appearing()
        {
            base.Appearing();
#pragma warning disable 4014
            this.AppearingAsync();
#pragma warning restore 4014
        }

        public async Task AppearingAsync()
        {
            await this.ActivityIndicatorState.WrapWithIndicator((Action)this.RefreshAccounts).Invoke();
        }

        private void RefreshAccounts()
        {
            var accounts = this.accountingService.GetAccounts();

            if (accounts != null)
            {
                var filtered = accounts.Where(x => !x.IsDeleted);
                this.AccountGroups = this.GroupAccounts(filtered);
            }
        }

        private async Task NavigateToAccountDetail(Account account)
        {
            if (account == null)
            {
                return;
            }

            await this.navigationService.Navigate<AccountsDetailPageViewModel, Account>(account);
        }

        private async Task DeleteAccount(Account account)
        {
            if (account == null)
            {
                return;
            }

            var confirmMsg = string.Format(AppResources.AccountsListPage_DeleteAccountConfirmMsgFormat, account.Name);

            var confirmed = await this.userInteraction.ConfirmAsync(
                          confirmMsg,
                          AppResources.AccountsListPage_DeleteAccountConfirmTitle,
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
                await this.userInteraction.AlertAsync(AppResources.AccountsListPage_DeleteAccountError);
            }
            finally
            {
                this.RefreshAccounts();
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