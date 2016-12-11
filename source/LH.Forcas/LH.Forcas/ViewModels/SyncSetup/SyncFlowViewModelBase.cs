using System;
using LH.Forcas.Extensions;
using Prism.Navigation;

namespace LH.Forcas.ViewModels.SyncSetup
{
    public abstract class SyncFlowViewModelBase : ViewModelBase
    {
        protected SyncFlowViewModelBase(NavigationParameters parameters)
        {
            this.FlowNavigationParameters = parameters;

            if (!parameters.ContainsKey(NavigationExtensions.SyncFlowStateParameterName))
            {
                throw new ArgumentException("SyncSetup pages must be navigated to with SyncFlowState parameter.", nameof(parameters));
            }

            this.State = (SyncFlowState)parameters[NavigationExtensions.SyncFlowStateParameterName];
        }

        protected SyncFlowState State { get; private set; }

        protected NavigationParameters FlowNavigationParameters { get; private set; }
    }
}