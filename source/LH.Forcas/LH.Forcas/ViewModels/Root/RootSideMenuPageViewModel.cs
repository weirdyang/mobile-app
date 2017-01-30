using System.Collections.Generic;
using LH.Forcas.Extensions;
using Prism.Navigation;

namespace LH.Forcas.ViewModels.Root
{
    using System;
    using Prism.Commands;

    public class RootSideMenuPageViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;

        public RootSideMenuPageViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
            this.Pages = NavigationExtensions.RootLevelPages;

            this.NavigateCommand = new DelegateCommand<NavigationPage>(this.Navigate);
        }

        public DelegateCommand<NavigationPage> NavigateCommand { get; private set; }

        public IEnumerable<NavigationPage> Pages { get; set; }

        private async void Navigate(NavigationPage page)
        {
            try
            {
                await page.NavigateAction.Invoke(this.navigationService);
            }
            catch (Exception ex)
            {
                // TODO: Log exception
                throw;
            }
        }
    }
}