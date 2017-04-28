using Xamarin.Forms;

namespace LH.Forcas.Views.Reusable.Controls
{
    public class ActivityOverlay : Frame
    {
        public static readonly BindableProperty ActivityTextProperty = BindableProperty.Create(
            nameof(ActivityText),
            typeof(string),
            typeof(ActivityOverlay));

        public string ActivityText
        {
            get { return (string) this.GetValue(ActivityTextProperty); }
            set { this.SetValue(ActivityTextProperty, value); }
        }

        public ActivityOverlay()
        {
            this.BackgroundColor = (OnPlatform<Color>)Application.Current.Resources["ActivityOverlayBackgroundColor"];

            var innerFrame = new Frame();
            innerFrame.BackgroundColor = (OnPlatform<Color>)Application.Current.Resources["ActivityOverlayInnerBackgroundColor"];
            innerFrame.HeightRequest = 80;
            innerFrame.WidthRequest = 300;
            innerFrame.VerticalOptions = LayoutOptions.Center;
            innerFrame.HorizontalOptions = LayoutOptions.Center;
            this.Content = innerFrame;

            var stack = new StackLayout();
            stack.Orientation = StackOrientation.Horizontal;
            stack.HorizontalOptions = LayoutOptions.StartAndExpand;
            innerFrame.Content = stack;
            
            var indicator = new ActivityIndicator();
            indicator.Margin = 10;
            indicator.HorizontalOptions = LayoutOptions.Center;
            indicator.VerticalOptions = LayoutOptions.Center;
            indicator.BindingContext = this;
            indicator.SetBinding(ActivityIndicator.IsRunningProperty, nameof(this.IsVisible), BindingMode.OneWay);
            stack.Children.Add(indicator);

            var label = new Label();
            label.FontSize = 15;
            label.VerticalOptions = LayoutOptions.Center;
            label.BindingContext = this;
            label.SetBinding(Label.TextProperty, nameof(this.ActivityText), BindingMode.OneWay);
            stack.Children.Add(label);
        }
    }
}