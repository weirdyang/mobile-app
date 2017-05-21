using MvvmCross.Plugins.Messenger;

namespace LH.Forcas.Events
{
    public class TrimMemoryRequestedEvent : MvxMessage
    {
        public TrimMemoryRequestedEvent(object sender) 
            : base(sender) { }

        public TrimMemorySeverity Severity { get; set; }
    }
}