using System;

namespace LH.Forcas.ViewModels
{
    public class BusyIndicatorSection : IDisposable
    {
        private int depthCounter = 1;
        private readonly ViewModelBase viewModel;

        public BusyIndicatorSection(ViewModelBase viewModel)
        {
            this.viewModel = viewModel;
            this.viewModel.IsBusy = true;
        }

        public void PushNested()
        {
            this.depthCounter++;
        }

        public void Dispose()
        {
            this.depthCounter--;

            if (this.depthCounter == 0)
            {
                this.viewModel.IsBusy = false;
            }
        }
    }
}