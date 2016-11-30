using System.Collections.Generic;
using System.Threading.Tasks;

namespace LH.Forcas.Services
{
    public interface IRefDataService
    {
        Task<IEnumerable<T>> GetRefDataAsync<T>() where T : new();

        Task UpdateRefDataAsync();
    }
}