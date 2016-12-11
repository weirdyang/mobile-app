using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.DeviceInfo;
using LH.Forcas.Extensions;
using LH.Forcas.Integration;
using LH.Forcas.Services;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;

namespace LH.Forcas.ViewModels.SyncSetup
{
    public class ProviderSelectionViewModel : SyncFlowViewModelBase
    {
        private readonly IConnectivity connectivity;
        private readonly IUnityContainer container;
        private readonly IUserSettingsService userSettingsService;
        private readonly IPageDialogService dialogService;

        private IFileSyncProvider selectedProvider;

        public ProviderSelectionViewModel(
            INavigationService navigationService, 
            IUnityContainer container, 
            IUserSettingsService userSettingsService, 
            IConnectivity connectivity, 
            IPageDialogService dialogService,
            NavigationParameters parameters)
            : base(parameters)
        {
            this.container = container;
            this.userSettingsService = userSettingsService;
            this.connectivity = connectivity;
            this.dialogService = dialogService;

            this.NavigateNextCommand = new DelegateCommand(
                async () => await navigationService.NavigateToSyncProviderAuthorizationAsync(this.SelectedProvider),
                () => this.SelectedProvider != null)
                .ObservesProperty(() => this.SelectedProvider);

            this.NavigateBackCommand = new DelegateCommand(
                async () => await this.State.NavigateBackFromFlowAction.Invoke(),
                () => this.State.NavigateBackFromFlowAction != null);
        }

        public bool IsBackButtonVisible => this.State.NavigateBackFromFlowAction != null;

        public IFileSyncProvider SelectedProvider
        {
            get { return this.selectedProvider; }
            set { this.SetProperty(ref this.selectedProvider, value); }
        }

        public IEnumerable<IFileSyncProvider> Providers { get; private set; }

        public ICommand NavigateNextCommand { get; }

        public ICommand NavigateBackCommand { get; }

        public override async Task OnNavigatedToAsync(NavigationParameters parameters)
        {
            using (this.StartBusyIndicator())
            {
                this.Providers = this.container.ResolveAll<IFileSyncProvider>();

                if (!string.IsNullOrEmpty(this.userSettingsService.SyncProviderName))
                {
                    this.SelectedProvider =
                        this.Providers.Single(x => x.Key == this.userSettingsService.SyncProviderName);
                }

                await this.VerifyInternetConnectivity();
            }
        }

        public override Task<bool> CanNavigateAsync(NavigationParameters parameters)
        {
            return Task.FromResult(this.SelectedProvider != null);
        }

        private async Task VerifyInternetConnectivity()
        {
            while (this.connectivity.InternetReachability == NetworkReachability.NotReachable)
            {
                var shouldRetry = await this.dialogService.DisplayAlertAsync(
                    "Internet connection",
                    "The internet connections seems to be unavailable. You can skip this step for now and change this later in the settings section or retry.",
                    "Skip",
                    "Retry");

                if (shouldRetry)
                {
                    // Wait for a couple of seconds and show is busy
                    // Then check again and follow the same logic
                }
                else
                {
                    this.State.FlowEndAction.Invoke();
                    return;
                }
            }
        }
    }
}