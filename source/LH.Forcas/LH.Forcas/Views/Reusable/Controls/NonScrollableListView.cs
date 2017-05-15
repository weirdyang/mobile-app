using System;
using Xamarin.Forms;

namespace LH.Forcas.Views.Reusable.Controls
{
    public class NonScrollableListView : ListView
    {
        public static readonly BindableProperty ItemsCountProperty = BindableProperty.Create(
           nameof(ItemsCount),
           typeof(int),
           typeof(NonScrollableListView),
           0,
           propertyChanged: UpdateRequestedHeight);

        public NonScrollableListView()
            : base(ListViewCachingStrategy.RecycleElement)
        {
        }

        public int ItemsCount
        {
            get { return (int)this.GetValue(ItemsCountProperty); }
            set { this.SetValue(ItemsCountProperty, value); }
        }

        private static void UpdateRequestedHeight(BindableObject bindable, object oldValue, object newValue)
        {
            var listView = (NonScrollableListView)bindable;
            
            if (listView.RowHeight < 0)
            {
                throw new InvalidOperationException("The RowHeight has to be set explicitly to use the NonScrollableListView control.");
            }

            var adjust = Device.RuntimePlatform != Device.Android ? -1 : listView.ItemsCount / 3 - 1;
            listView.HeightRequest = listView.ItemsCount * listView.RowHeight + adjust;
        }
    }
}