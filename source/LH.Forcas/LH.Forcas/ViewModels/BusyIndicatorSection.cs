using System;

namespace LH.Forcas.ViewModels
{
    public class BusyIndicatorSection : IDisposable
    {
        private readonly ViewModelBase viewModel;

        public BusyIndicatorSection(ViewModelBase viewModel)
        {
            this.viewModel = viewModel;
            this.viewModel.IsBusy = true;
        }

        public void Dispose()
        {
            this.viewModel.IsBusy = false;
        }
    }
}