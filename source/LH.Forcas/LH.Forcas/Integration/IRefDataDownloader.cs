using System;
using System.Threading.Tasks;
using LH.Forcas.Domain.RefData;

namespace LH.Forcas.Integration
{
    public interface IRefDataDownloader
    {
        Task<IRefDataUpdate[]> GetRefDataUpdates(DateTime? lastSyncTime);
    }
}