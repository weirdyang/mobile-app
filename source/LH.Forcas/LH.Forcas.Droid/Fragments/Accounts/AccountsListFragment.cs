using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using LH.Forcas.ViewModels.Accounts;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V4;

namespace LH.Forcas.Droid.Fragments.Accounts
{
    public class AccountsListFragment : MvxFragment<AccountsListViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.accounts_list, null);

            this.HasOptionsMenu = true;
            this.Activity.Title = this.Resources.GetString(Resource.String.accounts_title);
            //var compatActivity = (AppCompatActivity) this.Activity;
            //compatActivity.SupportActionBar.Title = this.Resources.GetString(Resource.String.accounts_title);

            return view;
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.accounts_list_toolbar_menu, menu);
            base.OnCreateOptionsMenu(menu, inflater);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            return base.OnOptionsItemSelected(item);
        }
    }
}