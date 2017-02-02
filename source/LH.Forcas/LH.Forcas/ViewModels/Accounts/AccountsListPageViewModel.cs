namespace LH.Forcas.ViewModels.Accounts
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.UserData;
    using Extensions;
    using Localization;
    using Prism.Commands;
    using Prism.Navigation;
    using Prism.Services;
    using Services;

    public class AccountsListPageViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IPageDialogService dialogService;
        private readonly IAccountingService accountingService;
        private readonly Type[] accountTypeOrder = { typeof(CashAccount), typeof(BankAccount), typeof(CreditCardAccount), typeof(LoanAccount), typeof(InvestmentAccount) };

        private IEnumerable<AccountsGroup> accounts;

        public AccountsListPageViewModel(
            IAccountingService accountingService,
            INavigationService navigationService, 
            IPageDialogService dialogService)
        {
            this.accountingService = accountingService;
            this.navigationService = navigationService;
            this.dialogService = dialogService;

            this.NavigateToAddAccountCommand = DelegateCommand.FromAsyncHandler(this.navigationService.NavigateToAccountAdd);
            this.NavigateToAccountDetailCommand = DelegateCommand<Account>.FromAsyncHandler(this.NavigateToAccountDetail);

            this.DeleteAccountCommand = DelegateCommand<Account>.FromAsyncHandler(this.DeleteAccount);

            this.RefreshAccountsCommand = new DelegateCommand(this.RefreshAccounts);
        }

        public DelegateCommand NavigateToAddAccountCommand { get; private set; }

        public DelegateCommand<Account> NavigateToAccountDetailCommand { get; private set; }

        public DelegateCommand RefreshAccountsCommand { get; private set; }

        public DelegateCommand<Account> DeleteAccountCommand { get; private set; }

        public IEnumerable<AccountsGroup> Accounts
        {
            get { return this.accounts; }
            private set { this.SetProperty(ref this.accounts, value); }
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            this.RefreshAccounts();
        }

        private void RefreshAccounts()
        {
            this.RunAsyncWithBusyIndicator(() =>
                                           {
                                               var groupped = this.accountingService.GetAccounts()
                                                                .GroupBy(account => account.GetType())
                                                                .Select(group => new AccountsGroup(group.Key, group.OrderBy(acc => acc.Name)))
                                                                .OrderBy(group => Array.IndexOf(this.accountTypeOrder, group.AccountType));

                                               this.Accounts = groupped;
                                           });
        }

        private async Task NavigateToAccountDetail(Account account)
        {
            await this.navigationService.NavigateToAccountDetail(account.AccountId);
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
                this.accountingService.DeleteAccount(account.AccountId);
            }
            catch (Exception ex)
            {
                // TODO: Log the exception
                Debug.WriteLine(ex);

                await this.dialogService.DisplayErrorAlert(AppResources.AccountsListPage_DeleteAccountError);
            }
            finally
            {
                this.RefreshAccounts();
            }
        }

        public class AccountsGroup : IOrderedEnumerable<Account>
        {
            private readonly IOrderedEnumerable<Account> accounts;

            public AccountsGroup(Type accountType, IOrderedEnumerable<Account> accounts)
            {
                this.AccountType = accountType;
                this.accounts = accounts;
            }

            public Type AccountType { get; }

            public IOrderedEnumerable<Account> CreateOrderedEnumerable<TKey>(Func<Account, TKey> keySelector, IComparer<TKey> comparer, bool descending)
            {
                return this.accounts.CreateOrderedEnumerable(keySelector, comparer, descending);
            }

            public IEnumerator<Account> GetEnumerator()
            {
                return this.accounts.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }
    }
}