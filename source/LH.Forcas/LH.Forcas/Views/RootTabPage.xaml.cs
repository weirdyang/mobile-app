using System;
using System.Linq;
using LH.Forcas.ViewModels;
using Prism.Mvvm;
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
        }

        private RootTabPageViewModel ViewModel => (RootTabPageViewModel)this.BindingContext;

        private void OnCurrentPageChanged(object sender, EventArgs eventArgs)
        {
            var navPage = this.CurrentPage as NavigationPage;

            var viewModel = navPage != null
                ? navPage.CurrentPage.BindingContext
                : this.CurrentPage.BindingContext;

            if (viewModel != null && !(viewModel is RootTabPageViewModel))
            {
                this.ViewModel.HandlePageSelectionChanged((ViewModelBase) viewModel);
            }
        }

        private void OnBindingContextChanged(object sender, EventArgs eventArgs)
        {
            var viewModel = (RootTabPageViewModel) this.BindingContext;
            viewModel.PageSelectionChangeRequested += this.HandlePageSelectionChangeRequested;
        }

        private void HandlePageSelectionChangeRequested(object sender, ViewModelEventArgs viewModelEventArgs)
        {
            this.CurrentPage = this.Children
                .Single(x => x.BindingContext == viewModelEventArgs.ViewModel);
        }
    }
}