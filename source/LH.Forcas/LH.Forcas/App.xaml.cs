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
    using Views.Accounts;

    public partial class App
    {
        public static CultureInfo CurrentCultureInfo { get; private set; }

        protected override void OnInitialized()
        {
            this.InitializeComponent();

            NavigationExtensions.InitializeNavigation();

            this.Container.Resolve<IPathResolver>().Initialize();
            this.Container.Resolve<IDbManager>().Initialize();

            CurrentCultureInfo = this.Container.Resolve<ILocale>().GetCultureInfo();

            this.NavigationService.NavigateToDashboard();
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
            this.Container.RegisterTypeForNavigation<AccountsAddTypeSelectionPage>();
            this.Container.RegisterTypeForNavigation<AccountsAddBankSelectionPage>();
        }
    }
}
