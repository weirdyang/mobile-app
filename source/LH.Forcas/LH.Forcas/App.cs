using System.Globalization;
using LH.Forcas.Localization;
using LH.Forcas.RefDataContract.Parsing;
using LH.Forcas.Services;
using LH.Forcas.Storage;
using LH.Forcas.Storage.Caching;
using LH.Forcas.Sync.RefData;
using LH.Forcas.ViewModels.Dashboard;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

namespace LH.Forcas
{
    using MvvmCross.Plugins.Messenger;
    using ViewModels;

    public class App : MvxApplication
    {
        public static CultureInfo CurrentCultureInfo { get; private set; }

        public App()
        {
            this.InitializePlugins();

            Mvx.ConstructAndRegisterSingleton<IAppConfig, AppConfig>();

            Mvx.LazyConstructAndRegisterSingleton<IGitHubClientFactory, GitHubClientFactory>();
            Mvx.LazyConstructAndRegisterSingleton<IRefDataDownloader, RefDataDownloader>();
            Mvx.LazyConstructAndRegisterSingleton<IRefDataUpdateParser, RefDataUpdateParser>();

            Mvx.LazyConstructAndRegisterSingleton<IDbManager, DbManager>();
            Mvx.RegisterType<IRefDataRepository>(() => new RefDataRepositoryCache(new RefDataRepository(), Mvx.Resolve<IMvxMessenger>()));
            Mvx.RegisterType<IUserDataRepository>(() => new UserDataRepositoryCache(new UserDataRepository(Mvx.Resolve<IDbManager>()), Mvx.Resolve<IMvxMessenger>()));

            Mvx.LazyConstructAndRegisterSingleton<IAccountingService, AccountingService>();
            Mvx.LazyConstructAndRegisterSingleton<IRefDataService, RefDataService>();
            Mvx.LazyConstructAndRegisterSingleton<IUserSettingsService, UserSettingsService>();
            
        }

        public override void Initialize()
        {
            base.Initialize();

            CurrentCultureInfo = Mvx.Resolve<ILocale>().GetCultureInfo();

            Mvx.Resolve<IPathResolver>().Initialize();
            Mvx.Resolve<IDbManager>().ApplyMigrations();
            // Mvx.Resolve<IUserSettingsService>().Initialize();

            this.RegisterAppStart<RootViewModel>();
        }

        private void InitializePlugins()
        {
            MvvmCross.Plugins.Messenger.PluginLoader.Instance.EnsureLoaded();
            MvvmCross.Plugins.Visibility.PluginLoader.Instance.EnsureLoaded();
        }
    }
}