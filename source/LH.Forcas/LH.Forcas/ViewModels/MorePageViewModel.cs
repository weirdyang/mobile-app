using LH.Forcas.Extensions;
using Prism.Commands;
using Prism.Navigation;

namespace LH.Forcas.ViewModels
{
    public class MorePageViewModel : ViewModelBase
    {
        public MorePageViewModel(INavigationService navigationService)
        {
            this.NavigateToPreferencesCommand = new AsyncDelegateCommand(this, navigationService.NavigateToPreferences);
            this.NavigateToAboutCommand = new AsyncDelegateCommand(this, navigationService.NavigateToAbout);
            this.NavigateToLicenseCommand = new AsyncDelegateCommand(this, navigationService.NavigateToLicense);
        }

        public DelegateCommand NavigateToPreferencesCommand { get; }

        public DelegateCommand NavigateToAboutCommand { get; }

        public DelegateCommand NavigateToLicenseCommand { get; }
    }
}