using System;
using LH.Forcas.ViewModels.Accounts;
using Prism.Mvvm;
using Prism.Navigation;

namespace LH.Forcas.ViewModels
{
    public class RootTabPageViewModel : BindableBase, INavigationAware
    {
        public const string SelectedPageParameter = "Page";

        public const string AccountsPageName = "Accounts";

        private ViewModelBase selectedPageViewModel;
        private AccountsListPageViewModel accountsListPageViewModel;

        public RootTabPageViewModel(
            AccountsListPageViewModel accountsListPageViewModel)
        {
            this.AccountsListPageViewModel = accountsListPageViewModel;
        }

        public event EventHandler<ViewModelEventArgs> PageSelectionChangeRequested;

        public AccountsListPageViewModel AccountsListPageViewModel
        {
            get { return this.accountsListPageViewModel; }
            private set { this.SetProperty(ref this.accountsListPageViewModel, value); }
        }

        public void HandlePageSelectionChanged(ViewModelBase newViewModel)
        {
            this.selectedPageViewModel?.OnNavigatedFrom(null);
            newViewModel.OnNavigatedTo(null);

            this.selectedPageViewModel = newViewModel;
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
            this.selectedPageViewModel?.OnNavigatedFrom(null);
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            object value;
            if (parameters != null && parameters.TryGetValue(SelectedPageParameter, out value) && value != null)
            {
                switch ((string)value)
                {
                    case AccountsPageName:
                        this.HandlePageSelectionChanged(this.AccountsListPageViewModel);
                        break;
                }
            }
            else
            {
                this.selectedPageViewModel?.OnNavigatedTo(null);
            }
        }

        protected virtual void OnPageSelectionChangeRequested(BindableBase viewModel)
        {
            this.PageSelectionChangeRequested?.Invoke(this, new ViewModelEventArgs(viewModel));
        }
    }
}