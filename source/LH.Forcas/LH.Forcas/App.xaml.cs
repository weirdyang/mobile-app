using LH.Forcas.Storage;
using Prism.Unity;
using Microsoft.Practices.Unity;

namespace LH.Forcas
{
    using System;
    using LH.Forcas.Extensions;
    using LH.Forcas.Views;
    using LH.Forcas.Views.Dashboard;
    using LH.Forcas.Views.Root;

    public partial class App
    {
        protected override void OnInitialized()
        {
            this.InitializeComponent();

            NavigationExtensions.InitializeNavigation();

            this.Container.Resolve<IPathResolver>().Initialize();
            this.Container.Resolve<IDbManager>().Initialize();

            this.NavigationService.NavigateToDashboard();
        }

        protected override void RegisterTypes()
        {
            this.Container.RegisterType<IDbManager, DbManager>();

            this.Container.RegisterTypeForNavigation<RootSideMenuPage>();
            this.Container.RegisterTypeForNavigation<RootTabPage>();
            this.Container.RegisterTypeForNavigation<RootSideMenuPage>();

            this.Container.RegisterTypeForNavigation<DashboardPage>();
            this.Container.RegisterTypeForNavigation<DashboardNavigationPage>();
        }
    }
}
