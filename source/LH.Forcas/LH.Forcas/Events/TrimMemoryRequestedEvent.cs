using Prism.Events;

namespace LH.Forcas.Events
{
    public class TrimMemoryRequestedEvent : PubSubEvent<TrimMemorySeverity>
    {
    }
}