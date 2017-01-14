using System.Collections.Generic;
using LH.Forcas.Extensions;
using Prism.Navigation;

namespace LH.Forcas.ViewModels.Root
{
    using System;
    using System.Threading.Tasks;

    public class RootSideMenuPageViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private NavigationPage selectedPage;

        public RootSideMenuPageViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
            this.Pages = NavigationExtensions.RootLevelPages;
        }

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
                    this.Navigate();
                    //this.selectedPage.NavigateAction.Invoke(this.navigationService);
                }
            }
        }

        private async void Navigate()
        {
            try
            {
                await this.selectedPage.NavigateAction.Invoke(this.navigationService);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}