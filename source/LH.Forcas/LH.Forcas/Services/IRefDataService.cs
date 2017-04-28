using System.Collections.Generic;
using System.Threading.Tasks;
using LH.Forcas.RefDataContract;

namespace LH.Forcas.Services
{
    public interface IRefDataService
    {
        IList<Bank> GetBanks();

        IList<Country> GetCountries();

        Country GetCountry(string id);

        Currency GetCurrency(string id);

        IList<Currency> GetCurrencies();

        Task UpdateRefData();
    }
}