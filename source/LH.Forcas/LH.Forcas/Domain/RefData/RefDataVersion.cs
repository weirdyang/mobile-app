using LiteDB;

namespace LH.Forcas.Domain.RefData
{
    public class RefDataVersion
    {
        [BsonId]
        public string TypeName { get; set; }

        public int Version { get; set; }
    }
}