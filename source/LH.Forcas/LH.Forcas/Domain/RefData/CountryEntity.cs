using SQLite.Net.Attributes;

namespace LH.Forcas.Domain.RefData
{
    public class Country
    {
        [MaxLength(5)]
        [PrimaryKey]
        public string Code { get; set; }

        [MaxLength(5)]
        public string DefaultCurrencyShortCode { get; set; }
    }
}
