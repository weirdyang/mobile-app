namespace LH.Forcas.Views.Reusable.Controls
{
    using System.Windows.Input;
    using Xamarin.Forms;

    public class FloatingActionButtonView : View
    {
        public static readonly BindableProperty IconProperty = BindableProperty.Create(
            nameof(Icon),
            typeof(string),
            typeof(FloatingActionButtonView));

        public static readonly BindableProperty IconColorProperty = BindableProperty.Create(
            nameof(IconColor),
            typeof(Color),
            typeof(FloatingActionButtonView),
            Color.White);

        public static readonly BindableProperty BackgroundColorNormalProperty = BindableProperty.Create(
            nameof(BackgroundColorNormal),
            typeof(Color?),
            typeof(FloatingActionButtonView));

        public static readonly BindableProperty BackgroundColorPressedProperty = BindableProperty.Create(
            nameof(BackgroundColorPressed),
            typeof(Color?),
            typeof(FloatingActionButtonView));

        public static readonly BindableProperty CommandProperty = BindableProperty.Create(
            nameof(Command),
            typeof(ICommand),
            typeof(FloatingActionButtonView));

        public static readonly BindableProperty AttachedListViewProperty = BindableProperty.Create(
            nameof(AttachedListView),
            typeof(ListView),
            typeof(FloatingActionButtonView));

        public ICommand Command
        {
            get { return (ICommand)this.GetValue(CommandProperty); }
            set { this.SetValue(CommandProperty, value); }
        }

        public string Icon
        {
            get { return (string)this.GetValue(IconProperty); }
            set { this.SetValue(IconProperty, value); }
        }

        public Color IconColor
        {
            get { return (Color)this.GetValue(IconColorProperty); }
            set { this.SetValue(IconColorProperty, value); }
        }

        public Color? BackgroundColorNormal
        {
            get { return (Color)this.GetValue(BackgroundColorNormalProperty); }
            set { this.SetValue(BackgroundColorNormalProperty, value); }
        }

        public Color? BackgroundColorPressed
        {
            get { return (Color)this.GetValue(BackgroundColorPressedProperty); }
            set { this.SetValue(BackgroundColorPressedProperty, value); }
        }

        public ListView AttachedListView
        {
            get { return (ListView) this.GetValue(AttachedListViewProperty); }
            set { this.SetValue(AttachedListViewProperty, value); }
        }
    }
}