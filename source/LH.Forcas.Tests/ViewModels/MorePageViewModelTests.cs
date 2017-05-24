using LH.Forcas.ViewModels;
using LH.Forcas.ViewModels.About;
using LH.Forcas.ViewModels.Settings;
using Moq;
using MvvmCross.Core.Navigation;
using NUnit.Framework;

namespace LH.Forcas.Tests.ViewModels
{
    [TestFixture]
    public class MorePageViewModelTests
    {
        protected MoreViewModel ViewModel;
        protected Mock<IMvxNavigationService> NavigationServiceMock;

        [SetUp]
        public void Setup()
        {
            this.NavigationServiceMock = new Mock<IMvxNavigationService>();
            this.ViewModel = new MoreViewModel(this.NavigationServiceMock.Object);
        }

        public class WhenNavigating : MorePageViewModelTests
        {
            [Test]
            public void ThenShouldNavigateToAboutPage()
            {
                this.NavigationServiceMock.Setup(x => x.Navigate<AboutPageViewModel>()).ReturnsAwaitable();

                Assert.True(this.ViewModel.NavigateToAboutCommand.CanExecute());
                this.ViewModel.NavigateToAboutCommand.Execute();

                this.NavigationServiceMock.VerifyAll();
            }

            [Test]
            public void ThenShouldNavigateToLicensePage()
            {
                this.NavigationServiceMock.Setup(x => x.Navigate<LicensePageViewModel>()).ReturnsAwaitable();

                Assert.True(this.ViewModel.NavigateToLicenseCommand.CanExecute());
                this.ViewModel.NavigateToLicenseCommand.Execute();

                this.NavigationServiceMock.VerifyAll();
            }

            [Test]
            public void ThenShouldNavigateToPreferencesPage()
            {
                this.NavigationServiceMock.Setup(x => x.Navigate<SettingsPageViewModel>()).ReturnsAwaitable();

                Assert.True(this.ViewModel.NavigateToPreferencesCommand.CanExecute());
                this.ViewModel.NavigateToPreferencesCommand.Execute();

                this.NavigationServiceMock.VerifyAll();
            }
        }
    }
}