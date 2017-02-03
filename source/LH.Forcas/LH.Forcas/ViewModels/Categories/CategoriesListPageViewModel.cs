namespace LH.Forcas.ViewModels.Categories
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.UserData;
    using Extensions;
    using Localization;
    using Prism.Commands;
    using Prism.Navigation;
    using Prism.Services;
    using Services;

    public class CategoriesListPageViewModel : ViewModelBase
    {
        private readonly IPageDialogService dialogService;
        private readonly IAccountingService accountingService;

        private CategoryViewModel[] categories;

        public CategoriesListPageViewModel(
            IAccountingService accountingService,
            INavigationService navigationService,
            IPageDialogService dialogService)
        {
            this.accountingService = accountingService;
            this.dialogService = dialogService;

            this.NavigateToAddCategoryCommand = DelegateCommand.FromAsyncHandler(navigationService.NavigateToCategoriesAdd);

            this.DeleteCategoryCommand = DelegateCommand<Category>.FromAsyncHandler(this.DeleteCategory);
            this.RefreshCategoriesCommand = new DelegateCommand(this.RefreshCategories);
        }

        public DelegateCommand NavigateToAddCategoryCommand { get; private set; }

        public DelegateCommand RefreshCategoriesCommand { get; private set; }

        public DelegateCommand<Category> DeleteCategoryCommand { get; private set; }

        public CategoryViewModel[] Categories
        {
            get { return this.categories; }
            private set { this.SetProperty(ref this.categories, value); }
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            this.RefreshCategories();
        }

        private void RefreshCategories()
        {
            this.RunAsyncWithBusyIndicator(() =>
                                           {
                                               this.Categories = this.accountingService.GetCategories()
                                                    .OrderBy(x => x.Name)
                                                    .SelectMany(this.FlattenCategories)
                                                    .ToArray();

                                           });
        }

        private async Task DeleteCategory(Category category)
        {
            if (category == null)
            {
                return;
            }

            if (!await this.IsDeletionConfirmed(category))
            {
                return;
            }

            // Transaction move TBA - historical accuracy, data backward immutability...

            try
            {
                this.accountingService.DeleteCategory(category.CategoryId, null);
            }
            catch (Exception ex)
            {
                // TODO: Log the exception
                Debug.WriteLine(ex);

                await this.dialogService.DisplayErrorAlert(AppResources.AccountsListPage_DeleteAccountError);
            }
            finally
            {
                this.RefreshCategories();
            }
        }

        private async Task<bool> IsDeletionConfirmed(Category category)
        {
            // TODO: Differentiate the confirm message if the category is (not) root (e.g. add 'and all its subcategories)

            return await this.dialogService.DisplayConfirmAlert(
                                 AppResources.CategoriesListPage_DeleteCategoryConfirmTitle,
                                 AppResources.CategoriesListPage_DeleteCategoryConfirmMsgFormat,
                                 category.Name);
        }

        private IEnumerable<CategoryViewModel> FlattenCategories(Category rootLevel)
        {
            yield return new CategoryViewModel(rootLevel, true);

            if (rootLevel.Children != null)
            {
                foreach (var child in rootLevel.Children)
                {
                    yield return new CategoryViewModel(child, false);
                }
            }
        }

        public class CategoryViewModel
        {
            public CategoryViewModel(Category category, bool isRoot)
            {
                this.Category = category;
                this.IsRoot = isRoot;
            }
        
            public Category Category { get; private set; }

            public bool IsRoot { get; private set; }
        }
    }
}