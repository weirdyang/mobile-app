using System;
using System.Collections.Generic;
using Android.Support.Design.Widget;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Support.V7.AppCompat;

namespace LH.Forcas.Droid
{
    public static class Extensions
    {
        public static void RegisterBottomNavViewCommands<TVm>(this MvxCachingFragmentCompatActivity<TVm> activity, int resourceId, IDictionary<int, Func<TVm, IMvxCommand>> commands) 
            where TVm : class, IMvxViewModel
        {
            var view = activity.FindViewById<BottomNavigationView>(resourceId);

            view.NavigationItemSelected += (sender, args) =>
            {
                var command = commands[args.Item.ItemId].Invoke(activity.ViewModel);
                command.Execute();
            };
        }
    }
}