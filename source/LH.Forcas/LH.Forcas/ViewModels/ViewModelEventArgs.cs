using Prism.Mvvm;

namespace LH.Forcas.ViewModels
{
    public class ViewModelEventArgs
    {
        public ViewModelEventArgs(BindableBase viewModel)
        {
            this.ViewModel = viewModel;
        }

        public BindableBase ViewModel { get; private set; }
    }
}