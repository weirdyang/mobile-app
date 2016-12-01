using LH.Forcas.Domain.RefData;
using SQLiteNetExtensions.Attributes;

namespace LH.Forcas.Storage.Entities.RefData
{
    public class CountryEntity
    {
        public string Code { get; set; }

        [ForeignKey(typeof(CurrencyEntity))]
        public string DefaultCurrencyCode { get; set; }

        [ManyToOne]
        public CurrencyEntity DefaultCurrency { get; set; }
    }
}