using SQLite;

namespace LH.Forcas.Storage.Entities.RefData
{
    [Table("Currency")]
    public class CurrencyEntity
    {
        [PrimaryKey]
        public string ShortCode { get; set; }

        public string Name { get; set; }

        public string NumberFormat { get; set; }
    }
}
