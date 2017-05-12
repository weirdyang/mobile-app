using System;
using System.Linq;
using LH.Forcas.ViewModels;
using Xamarin.Forms;

namespace LH.Forcas.Views
{
    public partial class RootTabPage
    {
        public RootTabPage()
        {
            this.InitializeComponent();

            this.BindingContextChanged += this.OnBindingContextChanged;
            this.CurrentPageChanged += this.OnCurrentPageChanged;

            this.Appearing += (sender, args) => this.UpdatePropagatedProperties(this.CurrentPage);
        }

        private RootTabPageViewModel ViewModel => (RootTabPageViewModel)this.BindingContext;

        private void OnCurrentPageChanged(object sender, EventArgs eventArgs)
        {
            var viewModel = this.CurrentPage.BindingContext;

            this.UpdatePropagatedProperties(this.CurrentPage);

            if (viewModel != null && !(viewModel is RootTabPageViewModel))
            {
                this.ViewModel.HandlePageSelectionChanged((ViewModelBase)viewModel);
            }
        }

        private void UpdatePropagatedProperties(Page page)
        {
            this.Title = page.Title;
            NavigationPage.SetHasNavigationBar(this, NavigationPage.GetHasNavigationBar(page));
        }

        private void OnBindingContextChanged(object sender, EventArgs eventArgs)
        {
            var viewModel = (RootTabPageViewModel)this.BindingContext;
            viewModel.PageSelectionChangeRequested += this.HandlePageSelectionChangeRequested;
        }

        private void HandlePageSelectionChangeRequested(object sender, ViewModelEventArgs viewModelEventArgs)
        {
            this.CurrentPage = this.Children
                .Single(x => x.BindingContext == viewModelEventArgs.ViewModel);
        }
    }
}