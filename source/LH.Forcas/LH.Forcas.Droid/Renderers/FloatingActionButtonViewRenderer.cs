using LH.Forcas.Droid.Renderers;
using LH.Forcas.Views.Reusable.Controls;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(FloatingActionButtonView), typeof(FloatingActionButtonViewRenderer))]

namespace LH.Forcas.Droid.Renderers
{
    using System;
    using System.ComponentModel;
    using System.Windows.Input;
    using Android.Support.Design.Widget;
    using Android.Widget;
    using Views.Reusable.Controls;
    using Xamarin.Forms.Platform.Android;

    public class FloatingActionButtonViewRenderer : ViewRenderer<FloatingActionButtonView, FrameLayout>
    {
        private ICommand attachedCommand;

        private readonly FloatingActionButton button;

        public FloatingActionButtonViewRenderer()
        {
            this.button = new FloatingActionButton(Forms.Context);
            this.button.Click += this.HandleButtonClicked;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<FloatingActionButtonView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || this.Element == null)
                return;

            if (e.OldElement != null)
                e.OldElement.PropertyChanged -= this.HandlePropertyChanged;

            if (this.Element != null)
            {
                this.Element.PropertyChanged += this.HandlePropertyChanged;
            }

            this.SetIcon();
            this.HandleCommandChanged();

            var frame = new FrameLayout(this.Context);
            // frame.RemoveAllViews();
            frame.AddView(this.button);

            this.SetNativeControl(frame);
        }

        private void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Content":
                    this.Tracker.UpdateLayout();
                    break;

                case nameof(FloatingActionButtonView.Icon):
                case nameof(FloatingActionButtonView.IconColor):
                    this.SetIcon();
                    break;

                case nameof(FloatingActionButtonView.BackgroundColor):
                    if (this.Element.BackgroundColorNormal.HasValue)
                    {
                        this.button.SetBackgroundColor(this.Element.BackgroundColorNormal.Value.ToAndroid());
                    }
                    break;

                case nameof(FloatingActionButtonView.BackgroundColorPressed):
                    if (this.Element.BackgroundColorPressed.HasValue)
                    {
                        this.button.SetRippleColor(this.Element.BackgroundColorPressed.Value.ToAndroid());
                    }
                    break;

                case nameof(FloatingActionButtonView.Command):
                    this.HandleCommandChanged();
                    break;
            }
        }

        private void HandleButtonClicked(object sender, EventArgs e)
        {
            if (this.attachedCommand != null && this.button.Enabled)
            {
                this.attachedCommand.Execute(null);
            }
        }

        private void HandleCommandChanged()
        {
            if (this.attachedCommand != null)
            {
                this.attachedCommand.CanExecuteChanged -= this.HandleCommandCanExecuteChanged;
                this.attachedCommand = null;
            }

            if (this.Element.Command != null)
            {
                this.attachedCommand = this.Element.Command;
                this.attachedCommand.CanExecuteChanged += this.HandleCommandCanExecuteChanged;
            }
        }

        private void HandleCommandCanExecuteChanged(object sender, EventArgs eventArgs)
        {
            this.button.Enabled = this.attachedCommand.CanExecute(null);
        }

        private void SetIcon()
        {
            // TODO: Use standard image icon
            throw new NotImplementedException();
            //var icon = Plugin.Iconize.Iconize.FindIconForKey(this.Element.Icon);
            //if (icon == null)
            //{
            //    this.button.SetImageResource(Android.Resource.Color.Transparent);
            //    return;
            //}

            //var drawable = new IconDrawable(Forms.Context, icon)
            //    .Color(this.Element.IconColor.ToAndroid())
            //    .SizeDp((int)this.Element.HeightRequest);

            //this.button.SetScaleType(ImageView.ScaleType.FitCenter);
            //this.button.SetImageDrawable(drawable);
        }
    }
}