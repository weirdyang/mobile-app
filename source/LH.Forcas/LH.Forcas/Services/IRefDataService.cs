using System.Collections.Generic;
using LH.Forcas.Domain.RefData;

namespace LH.Forcas.Services
{
    public interface IRefDataService
    {
        IList<Bank> GetBanks();

        IList<Country> GetCountries();

        Currency GetCurrency(string id);

        IList<Currency> GetCurrencies();
    }
}