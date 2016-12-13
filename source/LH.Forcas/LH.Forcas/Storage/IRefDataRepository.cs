using System.Collections.Generic;
using LH.Forcas.Domain.RefData;

namespace LH.Forcas.Storage
{
    public interface IRefDataRepository
    {
        IEnumerable<Bank> GetBanks();

        IEnumerable<Currency> GetCurrencies();

        IEnumerable<Country> GetCountries();

        void SaveRefDataUpdates(IRefDataUpdate[] updates);
    }
}