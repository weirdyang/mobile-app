﻿using LH.Forcas.Contract;
using LH.Forcas.Views;
using Xamarin.Forms;

namespace LH.Forcas
{
    public partial class App : IApp
    {
        public const string LastRefDataSyncPropertyKey = "LastRefDataSync";

        public App()
        {
            this.InitializeComponent();

            this.Constants = new AppConstants();
            this.MainPage = new MainPage();
        }

        public IAppConstants Constants { get; }

        protected override void OnStart()
        {
            var dbManager = DependencyService.Get<IDbManager>();
            dbManager.Initialize();

            //var userDataRepository = DependencyService.Get<IUserDataRepository>();
            //var userPreferences = await userDataRepository.GetUserPreferencesAsync();

            //if (userPreferences == null)
            //{

            //}
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
