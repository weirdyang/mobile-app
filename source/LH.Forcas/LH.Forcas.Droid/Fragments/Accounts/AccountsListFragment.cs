using Android.OS;
using Android.Views;
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
            return this.BindingInflate(Resource.Layout.Accounts_List, null);
        }
    }
}