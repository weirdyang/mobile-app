using System;
using LH.Forcas.Events;

namespace LH.Forcas.Services
{
    public class TrimMemoryEventArgs : EventArgs
    {
        public TrimMemoryEventArgs(TrimMemorySeverity severity)
        {
            this.Severity = severity;
        }

        public TrimMemorySeverity Severity { get; }
    }
}