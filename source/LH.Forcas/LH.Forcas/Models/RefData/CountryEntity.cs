using SQLite;

namespace LH.Forcas.Models.RefData
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
