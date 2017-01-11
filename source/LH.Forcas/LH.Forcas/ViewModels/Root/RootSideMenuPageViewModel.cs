using System.Collections.Generic;
using LH.Forcas.Extensions;
using Prism.Navigation;

namespace LH.Forcas.ViewModels.Root
{
    public class RootSideMenuPageViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private NavigationPage selectedPage;

        public RootSideMenuPageViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
            this.Pages = NavigationExtensions.RootLevelPages;
        }

        //public event EventHandler RequestSideMenuClose;

        public IEnumerable<NavigationPage> Pages { get; set; }

        public NavigationPage SelectedPage
        {
            get { return this.selectedPage; }
            set
            {
                this.selectedPage = value;
                this.OnPropertyChanged();

                if (this.selectedPage != null)
                {
                    this.selectedPage.NavigateAction.Invoke(this.navigationService);
                }

                // this.OnRequestSideMenuClose();
            }
        }

        //protected virtual void OnRequestSideMenuClose()
        //{
        //    this.RequestSideMenuClose?.Invoke(this, EventArgs.Empty);
        //}
    }
}