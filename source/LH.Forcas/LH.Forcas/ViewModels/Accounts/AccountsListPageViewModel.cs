namespace LH.Forcas.ViewModels.Accounts
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
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

        private ObservableCollection<AccountsGroup> accountGroups;

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

        public ObservableCollection<AccountsGroup> AccountGroups
        {
            get { return this.accountGroups; }
            private set { this.SetProperty(ref this.accountGroups, value); }
        }

        public override async void OnNavigatedTo(NavigationParameters parameters)
        {
            await this.RunAsyncWithBusyIndicator((Action)this.RefreshAccounts);
        }

        private void RefreshAccounts()
        {    
            var accounts = this.accountingService.GetAccounts();
            var grouped = this.GroupAccounts(accounts);
            this.AccountGroups = new ObservableCollection<AccountsGroup>(grouped);
        }

        private async Task NavigateToAccountDetail(Account account)
        {
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
                // TODO: Log the exception
                Debug.WriteLine(ex);

                await this.dialogService.DisplayErrorAlert(AppResources.AccountsListPage_DeleteAccountError);
            }
            finally
            {
                this.RefreshAccounts();
            }
        }

        private IEnumerable<AccountsGroup> GroupAccounts(IEnumerable<Account> accounts)
        {
            Task.Delay(3000).Wait();

            return accounts.GroupBy(account => account.GetType())
                           .Select(group => new AccountsGroup(group.Key, group.OrderBy(acc => acc.Name)))
                           .OrderBy(group => Array.IndexOf(this.accountTypeOrder, group.AccountType));
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