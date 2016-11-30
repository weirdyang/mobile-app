using System;
using System.Threading.Tasks;
using LH.Forcas.Models.RefData;

namespace LH.Forcas.Contract
{
    public interface IRefDataDownloader
    {
        Task<IRefDataUpdate[]> GetRefDataUpdates(DateTime? lastSyncTime);
    }
}