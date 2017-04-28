namespace LH.Forcas.Views.Reusable.Behaviors
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using Xamarin.Forms;

    public class DynamicTableViewBehavior : Behavior<TableView>
    {
        #region TableSectionProperty

        public static readonly BindableProperty IsVisibleProperty = BindableProperty.CreateAttached(
          "IsVisible",
          typeof(bool),
          typeof(DynamicTableViewBehavior),
          true);

        public static bool GetIsVisible(BindableObject target)
        {
            return (bool)target.GetValue(IsVisibleProperty);
        }

        public static void SetIsVisible(BindableObject target, bool value)
        {
            target.SetValue(IsVisibleProperty, value);
        }

        #endregion

        private bool hasAppeared;
        private Page page;
        private TableView tableView;
        private Queue<Tuple<TableSection, bool>> queue;
        private IList<TableSection> originalSections;

        protected override void OnAttachedTo(TableView bindable)
        {
            this.queue = new Queue<Tuple<TableSection, bool>>();

            this.tableView = bindable;
            this.tableView.Root.CollectionChanged += this.SectionsCollectionChanged;
        }

        protected override void OnDetachingFrom(TableView bindable)
        {
            foreach (var section in this.originalSections)
            {
                section.PropertyChanged -= this.HandleSectionPropertyChanged;
            }

            this.tableView = null;
        }

        private void SectionsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (TableSection section in e.NewItems)
                {
                    section.PropertyChanged += this.HandleSectionPropertyChanged;
                }
            }
        }

        private void HandleSectionPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsVisible")
            {
                var section = (TableSection)sender;
                var shouldBeVisible = GetIsVisible(section);

                if (this.page == null)
                {
                    this.originalSections = CopySections(this.tableView);

                    this.page = GetParentPage(this.tableView);
                    this.page.Appearing += (o, args) =>
                    {
                        this.hasAppeared = true;
                        this.ProcessQueuesVisibilityChanges();
                    };
                }

                if (!this.hasAppeared)
                {
                    // The page has not been initialized yet
                    this.queue.Enqueue(new Tuple<TableSection, bool>(section, shouldBeVisible));
                }
                else
                {
                    this.UpdateSectionVisibility(section, shouldBeVisible);
                }
            }
        }

        private void ProcessQueuesVisibilityChanges()
        {
            while (this.queue.Count > 0)
            {
                var change = this.queue.Dequeue();
                this.UpdateSectionVisibility(change.Item1, change.Item2);
            }
        }

        private void UpdateSectionVisibility(TableSection section, bool shouldBeVisible)
        {
            var isVisible = this.tableView.Root.Contains(section);

            if (shouldBeVisible && !isVisible)
            {
                var index = this.originalSections.IndexOf(section);

                if (index >= this.tableView.Root.Count)
                {
                    index = this.tableView.Root.Count;
                }

                this.tableView.Root.Insert(index, section);
            }
            else if (!shouldBeVisible && isVisible)
            {
                this.tableView.Root.Remove(section);
            }
        }

        private static Page GetParentPage(TableView tableView)
        {
            Element element = tableView;

            while (element.Parent != null && !(element.Parent is Page))
            {
                element = element.Parent;
            }

            return (Page)element.Parent;
        }

        private static TableSection[] CopySections(TableView tableView)
        {
            var sectionArray = new TableSection[tableView.Root.Count];
            tableView.Root.CopyTo(sectionArray, 0);

            return sectionArray;
        }
    }
}