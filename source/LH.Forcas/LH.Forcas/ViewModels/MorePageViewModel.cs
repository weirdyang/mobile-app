using LH.Forcas.Extensions;
using Prism.Commands;
using Prism.Navigation;

namespace LH.Forcas.ViewModels
{
    public class MorePageViewModel : ViewModelBase
    {
        public MorePageViewModel(INavigationService navigationService)
        {
            this.NavigateToPreferencesCommand = DelegateCommand.FromAsyncHandler(async () => await navigationService.NavigateToPreferences());
            this.NavigateToAboutCommand = DelegateCommand.FromAsyncHandler(async () => await navigationService.NavigateToAbout());
            this.NavigateToLicenseCommand = DelegateCommand.FromAsyncHandler(async () => await navigationService.NavigateToLicense());
        }

        public DelegateCommand NavigateToPreferencesCommand { get; private set; }

        public DelegateCommand NavigateToAboutCommand { get; private set; }

        public DelegateCommand NavigateToLicenseCommand { get; private set; }
    }
}