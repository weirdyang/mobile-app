namespace LH.Forcas.ViewModels.Categories
{
    using System.Threading.Tasks;
    using Prism.Navigation;
    using Prism.Services;

    public class CategoriesDetailPageViewModel : DetailViewModelBase
    {
        public CategoriesDetailPageViewModel(IPageDialogService dialogService) 
            : base(dialogService)
        {
        }        

        protected override Task Save()
        {
            return base.Save();
        }

        protected override bool CanSave()
        {
            return base.CanSave();
        }
    }
}