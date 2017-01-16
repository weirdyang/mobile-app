namespace LH.Forcas.ViewModels
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Prism.Navigation;

    public abstract class ItemSelectionViewModelBase<TItem> : ViewModelBase
    {
        private TItem selectedItem;
        private IEnumerable<TItem> items;

        public TItem SelectedItem
        {
            get { return this.selectedItem; }
            set
            {
                this.SetProperty(ref this.selectedItem, value);

                if (value != null && value.Equals(default(TItem)))
                {
                    this.OnItemSelected(value);
                }
            }
        }

        public IEnumerable<TItem> Items
        {
            get { return this.items; }
            private set { this.SetProperty(ref this.items, value); }
        }

        public override async Task OnNavigatedToAsync(NavigationParameters parameters)
        {
            await base.OnNavigatedToAsync(parameters);

            this.Items = await this.GetSelectionItems();
            this.SelectedItem = default(TItem);
        }

        protected abstract Task<IEnumerable<TItem>> GetSelectionItems();

        protected abstract void OnItemSelected(TItem item);
    }
}