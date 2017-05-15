using System;
using System.Reflection;
using System.Threading.Tasks;
using LH.Forcas.Extensions;
using Prism.Navigation;

namespace LH.Forcas.ViewModels.About
{
    public class LicensePageViewModel : ViewModelBase
    {
        private string licenseText;
        
        public string LicenseText
        {
            get { return this.licenseText; }
            set { this.SetProperty(ref this.licenseText, value); }
        }

        public override async Task OnNavigatingToAsync(NavigationParameters parameters)
        {
            await this.RunAsyncWithBusyIndicator((Action)this.LoadLicenseText);
        }

        private void LoadLicenseText()
        {
            var type = this.GetType();
            var assembly = type.GetTypeInfo().Assembly;

            var resourceName = type.GetSiblingResourceName("License.html");

            this.LicenseText = assembly.GetManifestResourceContentAsText(resourceName); ;
        }
    }
}