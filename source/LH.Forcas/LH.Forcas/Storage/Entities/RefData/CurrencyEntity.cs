using SQLite;

namespace LH.Forcas.Storage.Entities.RefData
{
    [Table("Currency")]
    public class CurrencyEntity
    {
        [PrimaryKey]
        [MaxLength(5)]
        public string ShortCode { get; set; }

        [MaxLength(25)]
        public string Name { get; set; }

        [MaxLength(25)]
        public string NumberFormat { get; set; }
    }
}
