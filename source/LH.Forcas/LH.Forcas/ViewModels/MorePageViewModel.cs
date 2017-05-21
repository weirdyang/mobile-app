using LH.Forcas.ViewModels.About;
using LH.Forcas.ViewModels.Settings;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace LH.Forcas.ViewModels
{
    public class MorePageViewModel : MvxViewModel
    {
        public MorePageViewModel(IMvxNavigationService navigationService)
        {
            this.ActivityIndicatorState = new ActivityIndicatorState();
            this.NavigateToPreferencesCommand = new MvxAsyncCommand(this.ActivityIndicatorState.WrapWithIndicator(navigationService.Navigate<SettingsPageViewModel>));
            this.NavigateToAboutCommand = new MvxAsyncCommand(this.ActivityIndicatorState.WrapWithIndicator(navigationService.Navigate<AboutPageViewModel>));
            this.NavigateToLicenseCommand = new MvxAsyncCommand(this.ActivityIndicatorState.WrapWithIndicator(navigationService.Navigate<LicensePageViewModel>));
        }

        public ActivityIndicatorState ActivityIndicatorState { get; private set; }

        public MvxAsyncCommand NavigateToPreferencesCommand { get; }

        public MvxAsyncCommand NavigateToAboutCommand { get; }

        public MvxAsyncCommand NavigateToLicenseCommand { get; }
    }
}