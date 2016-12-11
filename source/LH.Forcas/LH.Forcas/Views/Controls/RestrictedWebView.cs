using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace LH.Forcas.Views.Controls
{
    public class RestrictedWebView : WebView
    {
        public static readonly BindableProperty TargetUriProperty =
            BindableProperty.Create("TargetUri", typeof(Uri), typeof(RestrictedWebView));

        public static readonly BindableProperty TargetUriCommandProperty =
            BindableProperty.Create("TargetUriCommand", typeof(ICommand), typeof(RestrictedWebView));

        public RestrictedWebView()
        {
            this.Navigated += this.OnNavigated;
        }

        public string TargetUri
        {
            get { return (string) this.GetValue(TargetUriProperty); }
            set { this.SetValue(TargetUriProperty, value); }
        }

        public ICommand TargetUriCommand
        {
            get { return (ICommand)this.GetValue(TargetUriCommandProperty); }
            set { this.SetValue(TargetUriCommandProperty, value); }
        }

        private void OnNavigated(object sender, WebNavigatedEventArgs args)
        {
            if (!string.IsNullOrEmpty(this.TargetUri) && this.TargetUriCommand != null && args.Url.StartsWith(this.TargetUri))
            {
                this.TargetUriCommand.Execute(null);
            }
        }
    }
}