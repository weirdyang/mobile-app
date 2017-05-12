using LH.Forcas.ViewModels;
using LH.Forcas.Views.About;
using LH.Forcas.Views.Preferences;
using Moq;
using NUnit.Framework;
using Prism.Navigation;

namespace LH.Forcas.Tests.ViewModels
{
    [TestFixture]
    public class MorePageViewModelTests
    {
        protected MorePageViewModel ViewModel;
        protected Mock<INavigationService> NavigationServiceMock;

        [SetUp]
        public void Setup()
        {
            this.NavigationServiceMock = new Mock<INavigationService>();
            this.ViewModel = new MorePageViewModel(this.NavigationServiceMock.Object);
        }

        public class WhenNavigating : MorePageViewModelTests
        {
            [Test]
            public void ThenShouldNavigateToAboutPage()
            {
                this.NavigationServiceMock.SetupNavigation(nameof(AboutPage));

                Assert.IsTrue(this.ViewModel.NavigateToAboutCommand.CanExecute());
                this.ViewModel.NavigateToAboutCommand.Execute();

                this.NavigationServiceMock.VerifyAll();
            }

            [Test]
            public void ThenShouldNavigateToLicensePage()
            {
                this.NavigationServiceMock.SetupNavigation(nameof(LicensePage));

                Assert.IsTrue(this.ViewModel.NavigateToLicenseCommand.CanExecute());
                this.ViewModel.NavigateToLicenseCommand.Execute();

                this.NavigationServiceMock.VerifyAll();
            }

            [Test]
            public void ThenShouldNavigateToPreferencesPage()
            {
                this.NavigationServiceMock.SetupNavigation(nameof(PreferencesPage));

                Assert.IsTrue(this.ViewModel.NavigateToPreferencesCommand.CanExecute());
                this.ViewModel.NavigateToPreferencesCommand.Execute();

                this.NavigationServiceMock.VerifyAll();
            }
        }
    }
}