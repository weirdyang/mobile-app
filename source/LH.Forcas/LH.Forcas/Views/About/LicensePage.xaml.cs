using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace LH.Forcas.Views.About
{
    public partial class LicensePage
    {
        public LicensePage()
        {
            this.InitializeComponent();

            NavigationPage.SetHasBackButton(this, true);
            NavigationPage.SetHasNavigationBar(this, true);

            this.Appearing += this.OnAppearing;
        }

        private void OnAppearing(object sender, EventArgs eventArgs)
        {
            for (var i = 0; i < this.Navigation.NavigationStack.Count; i++)
            {
                Debug.WriteLine("{0}: {1}", this.GetType().Name, this.Navigation.NavigationStack[i].GetType().Name);
            }
        }
    }
}