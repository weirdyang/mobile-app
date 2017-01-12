using LiteDB;

namespace LH.Forcas.Domain.RefData
{
    public class Country
    {
        [BsonId]
        public string CountryCode { get; set; }

        public string DefaultCurrencyCode { get; set; }
    }
}