using LH.Forcas.Storage;
using LH.Forcas.Views;
using Prism.Unity;
using Microsoft.Practices.Unity;

namespace LH.Forcas
{
    public partial class App
    {
        // public const string LastRefDataSyncPropertyKey = "LastRefDataSync";

        protected override void OnInitialized()
        {
            this.InitializeComponent();

            this.NavigationService.NavigateAsync("MainPage");

            this.Container.Resolve<IDbManager>().Initialize();
        }

        protected override void RegisterTypes()
        {
            this.Container.RegisterTypeForNavigation<MainPage>();
        }
    }
}
