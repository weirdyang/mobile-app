namespace LH.Forcas.ViewModels.Accounts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Extensions;
    using Localization;
    using Prism.Commands;
    using Prism.Navigation;
    using Prism.Services;

    public class AccountsListPageViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IPageDialogService dialogService;

        private object selectedAccount;

        public AccountsListPageViewModel(INavigationService navigationService, IPageDialogService dialogService)
        {
            this.navigationService = navigationService;
            this.dialogService = dialogService;

            this.NavigateToAddAccountCommand = new DelegateCommand(
                async () => await this.navigationService.NavigateToAddAccount());

            this.RefreshAccountsCommand = new DelegateCommand(
                async () => await this.LoadAccounts());

            this.DeleteAccountCommand = new DelegateCommand<object>(
                async account => await this.DeleteAccount(account));
        }

        public ICommand NavigateToAddAccountCommand { get; private set; }

        public ICommand RefreshAccountsCommand { get; private set; }

        public ICommand DeleteAccountCommand { get; private set; }

        public IEnumerable<object> Accounts { get; private set; }

        public object SelectedAccount
        {
            get { return this.selectedAccount; }
            set
            {
                this.selectedAccount = value;
                this.OnPropertyChanged();

                // TODO: Navigate to detail
            }
        }

        public override async Task OnNavigatedToAsync(NavigationParameters parameters)
        {
            await this.LoadAccounts();
            this.SelectedAccount = null;
        }

        private Task LoadAccounts()
        {
            return Task.FromResult(0);
        }

        private async Task DeleteAccount(object account)
        {
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

            // TODO: Delete the account
        }
    }
}