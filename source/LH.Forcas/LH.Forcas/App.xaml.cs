using System;
using System.Globalization;
using System.Reflection;
using LH.Forcas.Analytics;
using LH.Forcas.Extensions;
using LH.Forcas.Localization;
using LH.Forcas.RefDataContract.Parsing;
using LH.Forcas.Services;
using LH.Forcas.Storage;
using LH.Forcas.Storage.Caching;
using LH.Forcas.Storage.Data;
using LH.Forcas.Sync.RefData;
using LH.Forcas.ViewModels.About;
using LH.Forcas.ViewModels.Accounts;
using LH.Forcas.Views;
using LH.Forcas.Views.About;
using LH.Forcas.Views.Accounts;
using LH.Forcas.Views.Categories;
using LH.Forcas.Views.Dashboard;
using LH.Forcas.Views.Preferences;
using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Unity;

namespace LH.Forcas
{
    public partial class App : IApp
    {
        public Version AppVersion { get; private set; }

        public static CultureInfo CurrentCultureInfo { get; private set; }

        public override void Initialize()
        {
            base.Initialize();

            this.SetVersion();
            this.InitializeComponent();

            this.Container.Resolve<IPathResolver>().Initialize();
            this.Container.Resolve<IDbManager>().ApplyMigrations();

            this.AmountToCurrencyStringConverter.RefDataService = this.Container.Resolve<IRefDataService>();

#if DEBUG
            TestData.InsertTestData(this.Container.Resolve<IUserDataRepository>());
#endif
            this.Container.Resolve<IDeviceService>().Initialize(this.Container.Resolve<IEventAggregator>());
            this.Container.Resolve<IUserSettingsService>().Initialize();

            CurrentCultureInfo = this.Container.Resolve<ILocale>().GetCultureInfo();
        }

#pragma warning disable 4014
        protected override void OnInitialized()
        {
            this.NavigationService.NavigateToDashboard()
                .ContinueWith(x =>
                                {
                                    if (x.Exception != null)
                                    {
                                        // TODO: Log and report the exception as fatal
                                        throw x.Exception;
                                    }
                                });
        }
#pragma warning restore 4014

        protected override void RegisterTypes()
        {
            this.Container.RegisterInstance(typeof(IApp), this, new ContainerControlledLifetimeManager());

            this.Container.RegisterType<IAnalyticsReporter, AnalyticsReporter>(new ContainerControlledLifetimeManager());

            this.Container.RegisterType<IGitHubClientFactory, GitHubClientFactory>(new ContainerControlledLifetimeManager());
            this.Container.RegisterType<IRefDataDownloader, RefDataDownloader>(new ContainerControlledLifetimeManager());
            this.Container.RegisterType<IRefDataUpdateParser, RefDataUpdateParser>(new ContainerControlledLifetimeManager());

            this.Container.RegisterType<IDbManager, DbManager>(new ContainerControlledLifetimeManager());

            this.Container.RegisterType<IRefDataRepository, RefDataRepository>("Repository", new ContainerControlledLifetimeManager());
            this.Container.RegisterType<IRefDataRepository, RefDataRepositoryCache>(new ContainerControlledLifetimeManager());

            this.Container.RegisterType<IUserDataRepository, UserDataRepository>("Repository", new ContainerControlledLifetimeManager());
            this.Container.RegisterType<IUserDataRepository, UserDataRepositoryCache>(new ContainerControlledLifetimeManager());

            this.Container.RegisterType<IAccountingService, AccountingService>(new ContainerControlledLifetimeManager());
            this.Container.RegisterType<IRefDataService, RefDataService>(new ContainerControlledLifetimeManager());
            this.Container.RegisterType<IUserSettingsService, UserSettingsService>(new ContainerControlledLifetimeManager());

            this.Container.RegisterTypeForNavigation<RootNavigationPage>();
            this.Container.RegisterTypeForNavigation<RootTabPage>();

            this.Container.RegisterTypeForNavigation<DashboardPage>();
            this.Container.RegisterTypeForNavigation<DashboardNavigationPage>();

            this.Container.RegisterTypeForNavigation<AccountsListPage, AccountsListPageViewModel>();
            this.Container.RegisterTypeForNavigation<AccountsDetailPage>();

            this.Container.RegisterTypeForNavigation<CategoriesListPage>();
            this.Container.RegisterTypeForNavigation<CategoriesDetailPage>();

            this.Container.RegisterTypeForNavigation<PreferencesPage>();
            this.Container.RegisterTypeForNavigation<AboutPage, AboutPageViewModel>();
            this.Container.RegisterTypeForNavigation<LicensePage, LicensePageViewModel>();
        }

        private void SetVersion()
        {
            var type = this.GetType().GetTypeInfo();
            var assemblyName = new AssemblyName(type.FullName);

            this.AppVersion = assemblyName.Version;
        }
    }
}