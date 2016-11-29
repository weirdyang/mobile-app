using System.Collections.Generic;
using System.Threading.Tasks;

namespace LH.Forcas.Contract
{
    public interface IRefDataRepository
    {
        Task<IEnumerable<T>> GetRefDataAsync<T>() where T : new();

        Task UpdateAsync();
    }
}