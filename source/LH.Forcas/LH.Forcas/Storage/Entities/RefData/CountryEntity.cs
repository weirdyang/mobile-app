using SQLite;

namespace LH.Forcas.Storage.Entities.RefData
{
    [Table("Country")]
    public class CountryEntity
    {
        [MaxLength(5)]
        [PrimaryKey]
        public string Code { get; set; }

        [MaxLength(5)]
        public string DefaultCurrencyShortCode { get; set; }
    }
}
