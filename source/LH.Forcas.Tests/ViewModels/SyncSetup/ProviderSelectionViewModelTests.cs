using System.Linq;
using System.Threading.Tasks;
using Acr.DeviceInfo;
using LH.Forcas.Extensions;
using LH.Forcas.Integration;
using LH.Forcas.Services;
using LH.Forcas.ViewModels.SyncSetup;
using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;
using Prism.Navigation;
using Prism.Services;

namespace LH.Forcas.Tests.ViewModels.SyncSetup
{
    [TestFixture]
    public class ProviderSelectionViewModelTests
    {
        private bool hasBackActionBeenCalled;
        private bool hasFlowEndActionBeenCalled;

        private SyncFlowState stateFromAppInit;
        private SyncFlowState stateFromSettingsPage;

        private IFileSyncProvider dummyFileSyncProvider;
        private Mock<IConnectivity> connectivityMock;
        private Mock<IUnityContainer> containerMock;
        private Mock<INavigationService> navigationServiceMock;
        private Mock<IUserSettingsService> userSettingsServiceMock;
        private Mock<IPageDialogService> pageDialogServiceMock;

        [SetUp]
        public void Setup()
        {
            this.hasFlowEndActionBeenCalled = false;
            this.hasBackActionBeenCalled = false;

            this.InitSyncFlowStates();

            this.dummyFileSyncProvider = new DummyFileSyncProvider();

            this.connectivityMock = new Mock<IConnectivity>();
            this.containerMock = new Mock<IUnityContainer>();
            this.navigationServiceMock = new Mock<INavigationService>();
            this.pageDialogServiceMock = new Mock<IPageDialogService>();
            this.userSettingsServiceMock = new Mock<IUserSettingsService>();

            this.containerMock
                .Setup(x => x.ResolveAll(typeof(IFileSyncProvider)))
                .Returns(new[] { this.dummyFileSyncProvider });

            this.connectivityMock
                .SetupGet(x => x.InternetReachability)
                .Returns(NetworkReachability.Wifi);            
        }

        [Test]
        public void BackActionShouldBeAvailableWhenGoingFromSettings()
        {
            var viewModel = this.CreateViewModel(this.stateFromSettingsPage);
            
            Assert.IsTrue(viewModel.IsBackButtonVisible);
            Assert.IsTrue(viewModel.NavigateBackCommand.CanExecute(null));
        }

        [Test]
        public void BackActionShouldCallDelegateFromState()
        {
            var viewModel = this.CreateViewModel(this.stateFromSettingsPage);

            viewModel.NavigateBackCommand.Execute(null);
            Assert.IsTrue(this.hasBackActionBeenCalled);
        }

        [Test]
        public void BackActionShouldNotBeAvailableWhenGoingFromAppIni()
        {
            var viewModel = this.CreateViewModel(this.stateFromAppInit);

            Assert.IsFalse(viewModel.IsBackButtonVisible);
            Assert.IsFalse(viewModel.NavigateBackCommand.CanExecute(null));
        }

        [Test]
        public void ShouldPresetProviderFromSettingsWhenAvailable()
        {
            this.userSettingsServiceMock.SetupGet(x => x.SyncProviderName).Returns("Dummy");

            var viewModel = this.CreateViewModel(this.stateFromAppInit);
            viewModel.OnNavigatedTo(null);

            Assert.IsNotNull(viewModel.SelectedProvider);
            Assert.AreEqual("Dummy", viewModel.SelectedProvider.Key);
        }

        [Test]
        public void ShouldNotAllowNavigateNextWhenSelectionIsEmpty()
        {
            var viewModel = this.CreateViewModel(this.stateFromAppInit);
            viewModel.OnNavigatedTo(null);

            viewModel.SelectedProvider = null;

            Assert.IsFalse(viewModel.NavigateNextCommand.CanExecute(null));
        }

        [Test]
        public void AllowNavigateNextShouldReactToSelectionChange()
        {
            var viewModel = this.CreateViewModel(this.stateFromAppInit);
            viewModel.OnNavigatedTo(null);

            viewModel.SelectedProvider = null;
            Assert.IsFalse(viewModel.NavigateNextCommand.CanExecute(null));

            viewModel.SelectedProvider = viewModel.Providers.First();
            Assert.IsTrue(viewModel.NavigateNextCommand.CanExecute(null));
        }

        [Test]
        public void ShouldNotAllowExternalNavigationWhenSelectionIsEmpty()
        {
            var viewModel = this.CreateViewModel(this.stateFromAppInit);
            viewModel.OnNavigatedTo(null);

            viewModel.SelectedProvider = null;

            Assert.IsFalse(viewModel.CanNavigateAsync(null).Result);
        }

        [Test]
        public void ShouldNavigateWithSelectedProvider()
        {
            this.navigationServiceMock.Setup(ns => ns.NavigateAsync(
                It.Is<string>(x => x == NavigationExtensions.ProviderAuthorizePage),
                It.Is<NavigationParameters>(x => x.ContainsKey("Provider")),
                null, 
                true));

            var viewModel = this.CreateViewModel(this.stateFromAppInit);
            viewModel.OnNavigatedTo(null);

            viewModel.SelectedProvider = viewModel.Providers.First();

            viewModel.NavigateNextCommand.Execute(null);

            this.navigationServiceMock.VerifyAll();
        }

        [Test]
        public void ShouldNavigateAwayIfUserSkipsFlow()
        {
            this.connectivityMock.Reset();
            this.connectivityMock.SetupGet(x => x.InternetReachability).Returns(NetworkReachability.NotReachable);

            this.pageDialogServiceMock.Setup(
                x => x.DisplayAlertAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            var viewModel = this.CreateViewModel(this.stateFromAppInit);
            viewModel.OnNavigatedTo(null);

            this.pageDialogServiceMock.VerifyAll();

            Assert.IsTrue(this.hasFlowEndActionBeenCalled);
        }

        [Test]
        public void ShouldRetryIfUserChoosesTo()
        {
            this.connectivityMock.Reset();
            this.connectivityMock.SetupGet(x => x.InternetReachability).Returns(NetworkReachability.NotReachable);

            this.pageDialogServiceMock
                .SetupSequence(x => x.DisplayAlertAsync(
                    It.IsAny<string>(), 
                    It.IsAny<string>(), 
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync(true)
                .ReturnsAsync(true)
                .ReturnsAsync(false);

            var viewModel = this.CreateViewModel(this.stateFromAppInit);
            viewModel.OnNavigatedTo(null);

            this.pageDialogServiceMock.VerifyAll();
        }

        private ProviderSelectionViewModel CreateViewModel(SyncFlowState state)
        {
            var parameters = new NavigationParameters();
            parameters.Add(NavigationExtensions.FlowParameterName, state);

            return new ProviderSelectionViewModel(
                this.navigationServiceMock.Object,
                this.containerMock.Object,
                this.userSettingsServiceMock.Object,
                this.connectivityMock.Object,
                this.pageDialogServiceMock.Object,
                parameters);
        }

        private void InitSyncFlowStates()
        {
            this.stateFromAppInit = new SyncFlowState
            {
                FlowEndAction = () => this.hasFlowEndActionBeenCalled = true
            };

            this.stateFromSettingsPage = new SyncFlowState
            {
                FlowEndAction = () => this.hasFlowEndActionBeenCalled = true,
                NavigateBackFromFlowAction = () =>
                {
                    this.hasBackActionBeenCalled = true;
                    return Task.FromResult(0);
                },
            };
        }

        private class DummyFileSyncProvider : IFileSyncProvider
        {
            public string Key => "Dummy";

            public string DisplayName => "Dummy Provider";
        }
    }
}
