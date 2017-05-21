using System.Reflection;
using System.Threading.Tasks;
using LH.Forcas.Extensions;
using MvvmCross.Core.ViewModels;

namespace LH.Forcas.ViewModels.About
{
    public class LicensePageViewModel : MvxViewModel
    {
        private string licenseText;

        public LicensePageViewModel()
        {
            this.ActivityIndicatorState = new ActivityIndicatorState();
        }

        public string LicenseText
        {
            get => this.licenseText;
            set => this.SetProperty(ref this.licenseText, value);
        }

        public ActivityIndicatorState ActivityIndicatorState { get; }

        public override void Appearing()
        {
#pragma warning disable 4014
            this.AppearingAsync();
#pragma warning restore 4014
        }

        public async Task AppearingAsync()
        {
            await this.ActivityIndicatorState.RunWithIndicator(this.LoadLicenseText);
        }

        private void LoadLicenseText()
        {
            var type = this.GetType();
            var assembly = type.GetTypeInfo().Assembly;

            var resourceName = type.GetSiblingResourceName("License.html");

            this.LicenseText = assembly.GetManifestResourceContentAsText(resourceName);
        }
    }
}