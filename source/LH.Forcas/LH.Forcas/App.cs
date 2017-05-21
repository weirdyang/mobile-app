using System.Globalization;
using LH.Forcas.Localization;
using LH.Forcas.RefDataContract.Parsing;
using LH.Forcas.Services;
using LH.Forcas.Storage;
using LH.Forcas.Storage.Caching;
using LH.Forcas.Sync.RefData;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

namespace LH.Forcas
{
    public class App : MvxApplication
    {
        public static CultureInfo CurrentCultureInfo { get; private set; }

        public App()
        {
            Mvx.ConstructAndRegisterSingleton<IAppConfig, AppConfig>();

            Mvx.LazyConstructAndRegisterSingleton<IGitHubClientFactory, GitHubClientFactory>();
            Mvx.LazyConstructAndRegisterSingleton<IRefDataDownloader, RefDataDownloader>();
            Mvx.LazyConstructAndRegisterSingleton<IRefDataUpdateParser, RefDataUpdateParser>();

            Mvx.ConstructAndRegisterSingleton<IDbManager, DbManager>();
            Mvx.RegisterType<IRefDataRepository>(() => new RefDataRepositoryCache(Mvx.Resolve<RefDataRepository>(), null));
            Mvx.RegisterType<IUserDataRepository>(() => new UserDataRepositoryCache(Mvx.Resolve<UserDataRepository>(), null));

            Mvx.ConstructAndRegisterSingleton<IAccountingService, AccountingService>();
            Mvx.ConstructAndRegisterSingleton<IRefDataService, RefDataService>();
            Mvx.ConstructAndRegisterSingleton<IUserSettingsService, UserSettingsService>();
        }

        public override void Initialize()
        {
            base.Initialize();

            CurrentCultureInfo = Mvx.Resolve<ILocale>().GetCultureInfo();

            Mvx.Resolve<IPathResolver>().Initialize();
            Mvx.Resolve<IDbManager>().ApplyMigrations();
            Mvx.Resolve<IUserSettingsService>().Initialize();
        }
    }
}