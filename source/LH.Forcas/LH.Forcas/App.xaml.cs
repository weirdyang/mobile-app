namespace LH.Forcas
{
    using System.Globalization;
    using LH.Forcas.Extensions;
    using LH.Forcas.Localization;
    using LH.Forcas.Services;
    using LH.Forcas.Views.Dashboard;
    using LH.Forcas.Views.Root;
    using Microsoft.Practices.Unity;
    using Prism.Unity;
    using Storage;
    using Storage.Data;
    using Views.Accounts;

    public partial class App
    {
        public static CultureInfo CurrentCultureInfo { get; private set; }

        protected override void OnInitialized()
        {
            this.InitializeComponent();

            NavigationExtensions.InitializeNavigation();

            this.Container.Resolve<IPathResolver>().Initialize();
            var dbManager = this.Container.Resolve<IDbManager>();
            dbManager.Initialize();

#if DEBUG
            TestData.InsertTestData(dbManager);
#endif

            CurrentCultureInfo = this.Container.Resolve<ILocale>().GetCultureInfo();

#pragma warning disable 4014
            this.NavigationService.NavigateToDashboard();
#pragma warning restore 4014
        }

        protected override void RegisterTypes()
        {
            this.Container.RegisterType<IDbManager, DbManager>(new ContainerControlledLifetimeManager());
            this.Container.RegisterType<IAccountingService, AccountingService>(new ContainerControlledLifetimeManager());
            this.Container.RegisterType<IRefDataService, RefDataService>(new ContainerControlledLifetimeManager());

            this.Container.RegisterType<IRefDataRepository, RefDataRepository>(new ContainerControlledLifetimeManager());
            this.Container.RegisterType<IRoamingDataRepository, RoamingDataRepository>(new ContainerControlledLifetimeManager());

            this.Container.RegisterTypeForNavigation<RootSideMenuPage>();
            this.Container.RegisterTypeForNavigation<RootTabPage>();
            this.Container.RegisterTypeForNavigation<RootSideMenuPage>();

            this.Container.RegisterTypeForNavigation<DashboardPage>();
            this.Container.RegisterTypeForNavigation<DashboardNavigationPage>();

            this.Container.RegisterTypeForNavigation<AccountsListPage>();
            this.Container.RegisterTypeForNavigation<AccountsDetailPage>();
            this.Container.RegisterTypeForNavigation<AccountsNavigationPage>();
        }
    }
}
