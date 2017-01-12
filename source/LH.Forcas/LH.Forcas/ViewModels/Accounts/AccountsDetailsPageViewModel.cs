namespace LH.Forcas.ViewModels.Accounts
{
    using Domain.UserData;
    using Prism.Navigation;
    using Services;

    public class AccountsDetailsPageViewModel : ViewModelBase
    {
        private readonly IAccountingService accountingService;
        private readonly INavigationService navigationService;

        public AccountsDetailsPageViewModel(IAccountingService accountingService, INavigationService navigationService)
        {
            this.accountingService = accountingService;
            this.navigationService = navigationService;
        }

        public Account Account { get; set; }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
        }
    }
}