using LH.Forcas.ViewModels.About;
using LH.Forcas.ViewModels.Settings;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace LH.Forcas.ViewModels
{
    public class MoreViewModel
    {
        public MoreViewModel(IMvxNavigationService navigationService)
        {
            //this.NavigateToPreferencesCommand = new MvxAsyncCommand(this.ActivityIndicatorState.WrapWithIndicator(navigationService.Navigate<SettingsPageViewModel>));
            //this.NavigateToAboutCommand = new MvxAsyncCommand(this.ActivityIndicatorState.WrapWithIndicator(navigationService.Navigate<AboutPageViewModel>));
            //this.NavigateToLicenseCommand = new MvxAsyncCommand(this.ActivityIndicatorState.WrapWithIndicator(navigationService.Navigate<LicensePageViewModel>));
        }

        public IMvxAsyncCommand NavigateToPreferencesCommand { get; }

        public IMvxAsyncCommand NavigateToAboutCommand { get; }

        public IMvxAsyncCommand NavigateToLicenseCommand { get; }
    }
}