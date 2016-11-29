using System.Collections.Generic;
using System.Threading.Tasks;

namespace LH.Forcas.Contract
{
    public interface IRefDataService
    {
        Task<IEnumerable<T>> GetRefDataAsync<T>() where T : new();

        Task UpdateRefDataAsync();
    }
}