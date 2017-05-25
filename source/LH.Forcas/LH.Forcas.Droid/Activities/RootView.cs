using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content.PM;
using Android.Support.V7.Widget;
using LH.Forcas.Droid.Fragments.Accounts;
using LH.Forcas.Droid.Fragments.Dashboard;
using LH.Forcas.ViewModels;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Support.V7.AppCompat;

namespace LH.Forcas.Droid.Activities
{
    [Activity(Label = "Forcas", Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/MainTheme")]
    public class RootView : MvxCachingFragmentCompatActivity<RootViewModel>
    {
        private readonly IDictionary<string, MvxFragment> fragments = new Dictionary<string, MvxFragment>();

        protected override void OnViewModelSet()
        {
            this.SetContentView(Resource.Layout.root);
            this.InitializeFragments();

            this.SetSupportActionBar(this.FindViewById<Toolbar>(Resource.Id.root_toolbar));

            this.RegisterBottomNavViewCommands(Resource.Id.root_bottom_navigation, new Dictionary<int, Func<RootViewModel, IMvxCommand>>
            {
                {  Resource.Id.action_root_menu_dashboard, x => x.SwitchToDashboardCommand },
                {  Resource.Id.action_root_menu_accounts, x => x.SwitchToAccountsCommand }
            });

            this.ViewModel.RequestSwitch += this.HandleTabSwitchRequest;

            this.Show(this.fragments.First().Key, true);
        }

        private void InitializeFragments()
        {
            this.fragments.Clear();

            var dashboardFrag = new DashboardFragment();
            dashboardFrag.ViewModel = this.ViewModel.DashboardViewModel;
            this.fragments.Add(RootViewModel.DashboardTabName, dashboardFrag);

            this.fragments.Add(RootViewModel.AccountsTabName, new AccountsListFragment());
        }

        private void Show(string tabName, bool isInitial = false)
        {
            using (var trans = this.SupportFragmentManager.BeginTransaction())
            {
                trans.SetCustomAnimations(Android.Resource.Animation.FadeIn,
                                          Android.Resource.Animation.FadeOut);

                var fragment = this.fragments[tabName];
                trans.Replace(Resource.Id.fragmentHost, fragment);

                if (!isInitial)
                {
                    trans.AddToBackStack(null);
                }

                trans.Commit();
            }
        }

        private void HandleTabSwitchRequest(object sender, string tabName)
        {
            this.Show(tabName);
        }
    }
}