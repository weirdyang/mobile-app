using LH.Forcas.Storage;
using LH.Forcas.Views;
using Prism.Unity;
using Microsoft.Practices.Unity;

namespace LH.Forcas
{
    public partial class App
    {
        protected override void OnInitialized()
        {
            this.InitializeComponent();

            this.Container.Resolve<IPathResolver>().Initialize();
            this.Container.Resolve<IDbManager>().Initialize();

            this.NavigationService.NavigateAsync("MainPage");
        }

        protected override void RegisterTypes()
        {
            this.Container.RegisterType<IDbManager, DbManager>();

            this.Container.RegisterTypeForNavigation<MainPage>();
        }
    }
}
