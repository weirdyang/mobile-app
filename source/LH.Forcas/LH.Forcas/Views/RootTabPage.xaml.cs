using System;
using System.Linq;
using LH.Forcas.ViewModels;
using Prism.Mvvm;

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
            if (this.CurrentPage.BindingContext.GetType() != typeof(RootTabPageViewModel))
            {
                this.ViewModel.HandlePageSelectionChanged((ViewModelBase) this.CurrentPage.BindingContext);
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