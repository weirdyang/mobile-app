using System;
using System.Threading.Tasks;

namespace LH.Forcas.ViewModels.SyncSetup
{
    public class SyncFlowState
    {
        public Action FlowEndAction { get; set; }

        public Func<Task> NavigateBackFromFlowAction { get; set; }
    }
}