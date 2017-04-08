using System.Collections.Generic;
using LH.Forcas.Domain.RefData;
using LH.Forcas.Sync.RefData;

namespace LH.Forcas.Storage
{
    public interface IRefDataRepository
    {
        RefDataStatus GetStatus();

        IEnumerable<Bank> GetBanks();

        IEnumerable<Currency> GetCurrencies();

        IEnumerable<Country> GetCountries();

        void SaveRefDataUpdate(RefDataUpdate update, RefDataStatus status);
    }
}