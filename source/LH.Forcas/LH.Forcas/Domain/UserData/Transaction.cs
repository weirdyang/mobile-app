using LiteDB;

namespace LH.Forcas.Domain.UserData
{
    public class Transaction
    {
        [BsonId]
        public string TransactionId { get; set; }

        public string CounterpartyName { get; set; }

        public string CounterpartyAccountNumber { get; set; }

        public string Memo { get; set; }

        public string VariableSymbol { get; set; }

        public string ConstantSymbol { get; set; }

        public string SpecificSymbol { get; set; }

        public TransactionType TransactionType { get; set; }
    }
}