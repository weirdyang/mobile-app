using LH.Forcas.ViewModels.About;
using NUnit.Framework;

namespace LH.Forcas.Tests.ViewModels.About
{
    [TestFixture]
    public class AboutPageViewModelTests
    {
        protected AboutPageViewModel ViewModel;

        [SetUp]
        public void Setup()
        {
            this.ViewModel = new AboutPageViewModel();
        }

        public class WhenNavigatingTo : AboutPageViewModelTests
        {
            [Test]
            public void ShouldLoadVersion()
            {
                this.ViewModel.OnNavigatingToAsync(null).Wait();

                Assert.IsNotNull(this.ViewModel.AppVersion);
            }

            [Test]
            public void ShouldLoadAuthor()
            {
                this.ViewModel.OnNavigatingToAsync(null).Wait();

                Assert.IsNotEmpty(this.ViewModel.Author);
            }

            [Test]
            public void ShouldLoadDependencies()
            {
                this.ViewModel.OnNavigatingToAsync(null).Wait();

                Assert.IsNotEmpty(this.ViewModel.Dependencies);
            }
        }
    }
}