using SQLite.Net.Attributes;

namespace LH.Forcas.Storage.Entities.RefData
{
    public class RefDataVersionEntity
    {
        [PrimaryKey]
        public string EntityTypeName { get; set; }

        [NotNull]
        public int Version { get; set; }
    }
}