using System.Collections;
using Xamarin.Forms;

namespace LH.Forcas.Views.Reusable.Controls
{
    public class PickerCell : ViewCell
    {
        public static readonly BindableProperty LabelTextProperty = BindableProperty.Create(
            nameof(LabelText),
            typeof(string),
            typeof(PickerCell));

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
           nameof(ItemsSource),
           typeof(IList),
           typeof(PickerCell));

        public static readonly BindableProperty DisplayPropertyProperty = BindableProperty.Create(
           nameof(DisplayProperty),
           typeof(string),
           typeof(PickerCell));

        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(
           nameof(SelectedItem),
           typeof(object),
           typeof(PickerCell),
           defaultBindingMode:BindingMode.TwoWay);

        public string LabelText
        {
            get { return (string)this.GetValue(LabelTextProperty); }
            set { this.SetValue(LabelTextProperty, value); }
        }

        public IList ItemsSource
        {
            get { return (IList)this.GetValue(ItemsSourceProperty); }
            set { this.SetValue(ItemsSourceProperty, value); }
        }

        public string DisplayProperty
        {
            get { return (string)this.GetValue(DisplayPropertyProperty); }
            set { this.SetValue(DisplayPropertyProperty, value); }
        }

        public object SelectedItem
        {
            get { return this.GetValue(SelectedItemProperty); }
            set { this.SetValue(SelectedItemProperty, value); }
        }

        public PickerCell()
        {
            var label = new Label();
            var picker = new Picker();
            
            var grid = new Grid();
            this.View = grid;

            label.BindingContext = this;
            label.VerticalOptions = LayoutOptions.Center;
            label.SetBinding(Label.TextProperty, nameof(this.LabelText), BindingMode.OneWay);

            picker.BindingContext = this;
            picker.SetValue(Grid.ColumnProperty, 1);
            picker.SetBinding(Picker.ItemsSourceProperty, nameof(this.ItemsSource), BindingMode.OneWay);
            picker.SetBinding(Picker.SelectedItemProperty, nameof(this.SelectedItem), BindingMode.TwoWay);
            // picker.SetBinding(Picker.DisplayPropertyProperty, nameof(this.DisplayProperty), BindingMode.OneWay);

            grid.Padding = new Thickness(15, 0, 0, 0);
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            grid.Children.Add(label);
            grid.Children.Add(picker);
        }
    }
}