using LH.Forcas.iOS.Renderers;
using LH.Forcas.Views.Reusable.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(NonScrollableListView), typeof(NonScrollableListViewRenderer))]

namespace LH.Forcas.iOS.Renderers
{
    public class NonScrollableListViewRenderer : ListViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);

            if (this.Control != null)
            {
                this.Control.ScrollEnabled = false;
            }
        }
    }
}