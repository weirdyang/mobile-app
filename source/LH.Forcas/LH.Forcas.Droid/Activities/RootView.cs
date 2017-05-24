using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content.PM;
using Android.Support.Design.Widget;
using LH.Forcas.Droid.Fragments.Accounts;
using LH.Forcas.Droid.Fragments.Dashboard;
using LH.Forcas.ViewModels;
using LH.Forcas.ViewModels.Dashboard;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platform;

namespace LH.Forcas.Droid.Activities
{
    [Activity(Label = "Forcas", Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/MainTheme")]
    public class RootView : MvxCachingFragmentCompatActivity<RootViewModel>
    {
        private readonly IDictionary<string, MvxFragment> fragments = new Dictionary<string, MvxFragment>();

        protected override void OnViewModelSet()
        {
            this.SetContentView(Resource.Layout.Root);
            this.InitializeFragments();

            this.FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation).NavigationItemSelected += this.HandleBottomBarClicked;
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

        private void HandleBottomBarClicked(object sender, BottomNavigationView.NavigationItemSelectedEventArgs args)
        {
            switch (args.Item.ItemId)
            {
                case Resource.Id.action_root_menu_dashboard:
                    this.ViewModel.SwitchToDashboardCommand.Execute();
                    break;

                case Resource.Id.action_root_menu_accounts:
                    this.ViewModel.SwitchToAccountsCommand.Execute();
                    break;
            }
        }

        private void HandleTabSwitchRequest(object sender, string tabName)
        {
            this.Show(tabName);
        }
    }
}