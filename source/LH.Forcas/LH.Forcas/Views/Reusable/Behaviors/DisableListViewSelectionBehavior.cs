namespace LH.Forcas.Views.Reusable.Behaviors
{
    using Xamarin.Forms;

    public class DisableListViewSelectionBehavior : Behavior<ListView>
    {
        protected override void OnAttachedTo(ListView bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.ItemSelected += this.HandleItemSelected;
        }

        protected override void OnDetachingFrom(ListView bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.ItemSelected -= this.HandleItemSelected;
        }

        private void HandleItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var listView = (ListView) sender;
            listView.SelectedItem = null;
        }
    }
}