using System.Collections.Generic;
using System.Threading.Tasks;
using LH.Forcas.Domain.RefData;

namespace LH.Forcas.Services
{
    public interface IRefDataService
    {
        Task<IList<Bank>> GetBanks();

        Task<IList<Country>> GetCountriesAsync();

        Task<IList<Currency>> GetCurrencies();
    }
}