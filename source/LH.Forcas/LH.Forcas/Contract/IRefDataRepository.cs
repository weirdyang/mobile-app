using System.Collections.Generic;
using System.Threading.Tasks;
using LH.Forcas.Models.RefData;

namespace LH.Forcas.Contract
{
    public interface IRefDataRepository
    {
        Task<IEnumerable<T>> GetRefDataAsync<T>() where T : new();

        Task SaveRefDataUpdates(IRefDataUpdate[] updates);
    }
}