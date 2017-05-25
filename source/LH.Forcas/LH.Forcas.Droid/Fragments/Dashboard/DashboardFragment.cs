using Android.OS;
using Android.Views;
using LH.Forcas.ViewModels.Dashboard;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V4;

namespace LH.Forcas.Droid.Fragments.Dashboard
{
    public class DashboardFragment : MvxFragment<DashboardViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            this.Activity.Title = this.Resources.GetString(Resource.String.dashboard_title);

            return this.BindingInflate(Resource.Layout.Dashboard, null);
        }
    }
}