namespace LH.Forcas.Views.Reusable.Controls
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using Xamarin.Forms;

    public class IconPicker : Entry
    {
        public static readonly BindableProperty ItemsProperty = BindableProperty.Create(
            nameof(Items),
            typeof(IEnumerable<object>),
            typeof(IconPicker));

        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(
            nameof(SelectedItem),
            typeof(object),
            typeof(IconPicker));

        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(
            nameof(ItemTemplate),
            typeof(DataTemplate),
            typeof(IconPicker));

        public static readonly BindableProperty DisplayPropertyProperty = BindableProperty.Create(
            nameof(DisplayProperty),
            typeof(string),
            typeof(IconPicker));

        public string DisplayProperty
        {
            get { return (string) this.GetValue(DisplayPropertyProperty); }
            set { this.SetValue(DisplayPropertyProperty, value); }
        }

        private readonly IconPickerPage pickerPage;

        public IconPicker()
        {
            this.Focused += this.HandleControlFocused;
            this.PropertyChanged += this.HandlePropertyChanged;

            this.pickerPage = new IconPickerPage();
            this.pickerPage.SetBinding(Page.TitleProperty, nameof(this.Placeholder));

            this.pickerPage.ListView.ItemSelected += this.HandleItemSelected;
            this.pickerPage.ListView.SetBinding(ListView.ItemTemplateProperty, nameof(this.ItemTemplate));
            this.pickerPage.ListView.SetBinding(ListView.SelectedItemProperty, nameof(this.SelectedItem), BindingMode.TwoWay);
        }

        public object SelectedItem
        {
            get { return this.GetValue(SelectedItemProperty); }
            set { this.SetValue(SelectedItemProperty, value); }
        }

        public IEnumerable<object> Items
        {
            get { return (IEnumerable<object>) this.GetValue(ItemsProperty); }
            set { this.SetValue(ItemsProperty, value); }
        }

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)this.GetValue(ItemTemplateProperty); }
            set { this.SetValue(ItemTemplateProperty, value); }
        }

        private void HandleControlFocused(object sender, FocusEventArgs e)
        {
            if (!this.IsPickerPageDisplayed())
            {
                this.Navigation.PushModalAsync(this.pickerPage);
            }
        }

        private void HandleItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (this.IsPickerPageDisplayed())
            {
                this.Navigation.PopModalAsync();
            }

            // TODO: Set the selected value as value of the entry
        }

        private void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(this.DisplayProperty):
                case nameof(this.SelectedItem):
                    this.Text = this.GetSelectedItemStringRepresentation();
                    break;
            }
        }

        private bool IsPickerPageDisplayed()
        {
            return this.Navigation.ModalStack.Count > 0 && this.Navigation.ModalStack[0] == this.pickerPage;
        }

        private string GetSelectedItemStringRepresentation()
        {
            if (this.SelectedItem == null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(this.DisplayProperty))
            {
                return this.SelectedItem.ToString();
            }

            var itemType = this.SelectedItem.GetType();
            var property = itemType.GetRuntimeProperties().FirstOrDefault(x => x.Name == this.DisplayProperty);

            if (property == null)
            {
                return this.SelectedItem.ToString();
            }

            return property.GetValue(this.SelectedItem)?.ToString();
        }
    }
}