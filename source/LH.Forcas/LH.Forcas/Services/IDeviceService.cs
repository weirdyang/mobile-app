using System;

namespace LH.Forcas.Services
{
    public interface IDeviceService
    {
        string CountryCode { get; }

        bool IsNetworkAvailable { get; }
    }
}