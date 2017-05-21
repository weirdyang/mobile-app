using LH.Forcas.ViewModels.About;
using NUnit.Framework;

namespace LH.Forcas.Tests.ViewModels.About
{
    [TestFixture]
    public class LicensePageViewModelTests
    {
        protected LicensePageViewModel ViewModel;

        [SetUp]
        public void Setup()
        {
            this.ViewModel = new LicensePageViewModel();
        }

        public class WhenNavigatingTo : LicensePageViewModelTests
        {
            [Test]
            public void ShouldLoadLicenseText()
            {
                this.ViewModel.AppearingAsync().Wait();

                Assert.IsNotEmpty(this.ViewModel.LicenseText);
                AssertEx.Contains("GNU", this.ViewModel.LicenseText);
            }
        }
    }
}