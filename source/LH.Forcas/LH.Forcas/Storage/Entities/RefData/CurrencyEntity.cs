using SQLite.Net.Attributes;

namespace LH.Forcas.Storage.Entities.RefData
{
    public class CurrencyEntity
    {
        [PrimaryKey]
        public string ShortCode { get; set; }

        public string DisplayName { get; set; }

        [MaxLength(5)]
        public string Symbol { get; set; }

        public short PreferedSymbolPosition { get; set; }
    }
}