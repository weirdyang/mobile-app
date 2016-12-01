using System.Collections.Generic;
using System.Threading.Tasks;
using LH.Forcas.Domain.RefData;

namespace LH.Forcas.Storage
{
    public interface IRefDataRepository
    {
        Task<IList<Bank>> GetBanksAsync();

        Task<IList<Currency>> GetCurrenciesAsync();

        Task<IList<Country>> GetCountriesAsync();

        Task SaveRefDataUpdates(IRefDataUpdate[] updates);
    }
}