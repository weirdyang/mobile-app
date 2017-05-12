namespace LH.Forcas.Views.Reusable.Behaviors
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Xamarin.Forms;

    public static class HideWhenElementScrolled
    {
        public static readonly BindableProperty AttachedListViewProperty = BindableProperty.CreateAttached(
                "AttachedListView",
                typeof(ListView),
                typeof(HideWhenElementScrolled),
                null,
                propertyChanged:HandleAttachedListViewChanged);

        private static readonly BindableProperty ObjectsToHideShowProperty = BindableProperty.CreateAttached(
                "ObjectsToHideShow",
                typeof(List<View>),
                typeof(HideWhenElementScrolled),
                null);

        public static ListView GetAttachedListView(BindableObject target)
        {
            return (ListView)target.GetValue(AttachedListViewProperty);
        }

        public static void SetAttachedListView(BindableObject target, ListView value)
        {
            target.SetValue(AttachedListViewProperty, value);
        }

        private static List<View> GetObjectsToHideShow(BindableObject target)
        {
            return (List<View>)target.GetValue(ObjectsToHideShowProperty);
        }

        private static void SetObjectsToHideShow(BindableObject target, List<View> value)
        {
            target.SetValue(ObjectsToHideShowProperty, value);
        }

        private static void HandleAttachedListViewChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var oldListView = oldvalue as ListView;
            if (oldListView != null)
            {
                oldListView.ItemDisappearing -= HandleListViewItemDisappearing;
                oldListView.ItemAppearing -= HandleListViewItemAppearing;
                GetObjectsToHideShow(oldListView).Remove((View)bindable);
            }

            var newListView = newvalue as ListView;
            if (newListView != null)
            {
                newListView.ItemDisappearing += HandleListViewItemDisappearing;
                newListView.ItemAppearing += HandleListViewItemAppearing;

                var attachedObjList = GetObjectsToHideShow(newListView);

                if (attachedObjList == null)
                {
                    attachedObjList = new List<View>();
                    SetObjectsToHideShow(newListView, attachedObjList);
                }

                attachedObjList.Add((View)bindable);
            }
        }

        private static void HandleListViewItemDisappearing(object sender, ItemVisibilityEventArgs e)
        {
            HandleListViewScrollChange(sender, e, true);
        }

        private static void HandleListViewItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            HandleListViewScrollChange(sender, e, false);
        }

        private static void HandleListViewScrollChange(object sender, ItemVisibilityEventArgs e, bool hide)
        {
            var listView = (ListView) sender;
            var sourceList = listView.ItemsSource as IList;

            if (sourceList == null)
            {
                throw new InvalidOperationException(
                    "The HideWhenElementScrolled behavior can only be used with a ListView whose ItemsSource implements the IList interface.");
            }

            if (sourceList.Count > 0 && sourceList[0] == e.Item)
            {
                var views = GetObjectsToHideShow(listView);

                foreach (var view in views)
                {
                    if (hide)
                    {
                        view.FadeTo(0, 200);
                        view.TranslateTo(0, 70, 200);
                    }
                    else
                    {
                        view.FadeTo(1, 200);
                        view.TranslateTo(0, 0, 200);
                    }
                }
            }
        }
    }
}