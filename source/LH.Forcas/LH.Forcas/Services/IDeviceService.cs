using Prism.Events;

namespace LH.Forcas.Services
{
    public interface IDeviceService
    {
        /// <summary>
        /// The initialize method is used to pass dependencies because Xamarin DependencyService 
        /// cannot resolve ctor injection parameters from the Unity container
        /// </summary>
        void Initialize(IEventAggregator eventAggregator);

        string CountryCode { get; }

        bool IsNetworkAvailable { get; }
    }
}