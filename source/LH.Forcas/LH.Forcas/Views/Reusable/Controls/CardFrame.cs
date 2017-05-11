using Xamarin.Forms;

namespace LH.Forcas.Views.Reusable.Controls
{
    public class CardFrame : Frame
    {
        public CardFrame()
        {
            this.Padding = 0;

            if (Device.OS == TargetPlatform.iOS)
            {
                this.HasShadow = false;
                this.OutlineColor = Color.Transparent;
                this.BackgroundColor = Color.Transparent;
            }
        }
    }
}