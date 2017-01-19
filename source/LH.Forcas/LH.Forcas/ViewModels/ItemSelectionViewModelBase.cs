namespace LH.Forcas.ViewModels
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Prism.Commands;
    using Prism.Navigation;

    public abstract class ItemSelectionViewModelBase<TItem> : ViewModelBase
    {
        private IEnumerable<TItem> items;

        protected ItemSelectionViewModelBase()
        {
            this.SelectItemCommand = new DelegateCommand<TItem>(this.SelectItemCommandExecute);
            this.PropertyChanged += (sender, e) => Debug.WriteLine("Prop. changed: {0}", e.PropertyName);
        }

        public IEnumerable<TItem> Items
        {
            get { return this.items; }
            private set { this.SetProperty(ref this.items, value); }
        }

        public ICommand SelectItemCommand { get; private set; }

        protected virtual bool RequiresDataRefresh => false;

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            this.RunAsyncWithBusyIndicator(() => this.Items = this.GetSelectionItems().Result);
        }

        protected abstract Task<IEnumerable<TItem>> GetSelectionItems();

        protected abstract void OnItemSelected(TItem item);

        private void SelectItemCommandExecute(TItem item)
        {
            if (item != null && !item.Equals(default(TItem)))
            {
                this.OnItemSelected(item);
            }
        }
    }
}