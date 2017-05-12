using Xamarin.Forms;

namespace LH.Forcas.Views.Reusable.Behaviors
{
    public class DisableListViewScrollBehavior : Behavior<ListView>
    {
        public static readonly BindableProperty ItemsCountProperty = BindableProperty.Create(
            nameof(ItemsCount),
            typeof(int),
            typeof(DisableListViewScrollBehavior),
            propertyChanged: UpdateRequestedHeight);

        private ListView listView;

        public int ItemsCount
        {
            get { return (int) this.GetValue(ItemsCountProperty); }
            set { this.SetValue(ItemsCountProperty, value); }
        }

        protected override void OnAttachedTo(ListView bindable)
        {
            base.OnAttachedTo(bindable);
            this.listView = bindable;
        }

        protected override void OnDetachingFrom(ListView bindable)
        {
            base.OnDetachingFrom(bindable);
            this.listView = null;
        }

        private static void UpdateRequestedHeight(BindableObject bindable, object oldValue, object newValue)
        {
            var behavior = (DisableListViewScrollBehavior) bindable;

            if (behavior.listView == null)
            {
                return;
            }

            var adjust = Device.OS != TargetPlatform.Android ? 1 : 0; // TODO: Handle for iOS -vm.AboutItems.Count + 1;
            behavior.listView.HeightRequest = behavior.ItemsCount*behavior.listView.RowHeight + adjust;
        }
    }
}