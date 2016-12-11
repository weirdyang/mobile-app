using Prism.Navigation;

namespace LH.Forcas.ViewModels.SyncSetup
{
    public class ProviderAuthorizeViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;

        public ProviderAuthorizeViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }
    }
}