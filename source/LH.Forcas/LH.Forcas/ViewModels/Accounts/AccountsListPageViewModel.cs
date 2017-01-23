namespace LH.Forcas.ViewModels.Accounts
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using System.Windows.Input;
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

        private Account selectedAccount;

        public AccountsListPageViewModel(
            IAccountingService accountingService,
            INavigationService navigationService, 
            IPageDialogService dialogService)
        {
            this.accountingService = accountingService;
            this.navigationService = navigationService;
            this.dialogService = dialogService;

            this.NavigateToAddAccountCommand = new DelegateCommand(
                async () => await this.navigationService.NavigateToAccountAdd());

            this.RefreshAccountsCommand = new DelegateCommand(this.RefreshAccounts);

            this.DeleteAccountCommand = new DelegateCommand<Account>(
                async account => await this.DeleteAccount(account));
        }

        public ICommand NavigateToAddAccountCommand { get; private set; }

        public ICommand RefreshAccountsCommand { get; private set; }

        public ICommand DeleteAccountCommand { get; private set; }

        public IEnumerable<Account> Accounts { get; private set; }

        public Account SelectedAccount
        {
            get { return this.selectedAccount; }
            set
            {
                this.selectedAccount = value;
                this.OnPropertyChanged();

                if (this.selectedAccount != null)
                {
                    this.NavigateToAccountDetail();
                }
            }
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            this.RefreshAccounts();
        }

        private void RefreshAccounts()
        {
            this.Accounts = this.accountingService.GetAccounts();
            this.SelectedAccount = null;
        }

        private async void NavigateToAccountDetail()
        {
            await this.navigationService.NavigateToAccountDetail(this.selectedAccount.AccountId);
        }

        private async Task DeleteAccount(Account account)
        {
            if (account == null)
            {
                return;
            }

            var confirmMsg = string.Format(AppResources.AccountsListPage_DeleteAccountConfirmMsgFormat, account);

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

                // This could be changed into a confirm box that would report the error to our storage
                await this.dialogService.DisplayAlertAsync(
                    AppResources.AlertDialog_ErrorTitle,
                    AppResources.AccountsListPage_DeleteAccountError,
                    AppResources.AlertDialog_OK
                    );
            }
            finally
            {
                this.RefreshAccounts();
            }
        }
    }
}