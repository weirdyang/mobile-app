using LiteDB;

namespace LH.Forcas.Domain.RefData
{
    public class Country
    {
        [BsonId]
        public string Code { get; set; }

        public string DefaultCurrencyCode { get; set; }
    }
}