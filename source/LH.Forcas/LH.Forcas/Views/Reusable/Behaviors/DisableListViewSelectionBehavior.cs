using System;
using Xamarin.Forms;

namespace LH.Forcas.Views.Reusable.Behaviors
{
    public class DisableListViewSelectionBehavior : Behavior<ListView>
    {
        protected override void OnAttachedTo(ListView bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.ItemTapped += this.HandleItemSelected;
        }

        protected override void OnDetachingFrom(ListView bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.ItemTapped -= this.HandleItemSelected;
        }

        private void HandleItemSelected(object sender, EventArgs e)
        {
            var listView = (ListView) sender;
            listView.SelectedItem = null;
        }
    }
}