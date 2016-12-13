using LiteDB;

namespace LH.Forcas.Domain.RefData
{
    public class Bank
    {
        [BsonId]
        public string BankId { get; set; }

        public string Name { get; set; }

        public string IbanFormat { get; set; }

        public string CountryCode { get; set; }

        public int RoutingCode { get; set; }
    }
}