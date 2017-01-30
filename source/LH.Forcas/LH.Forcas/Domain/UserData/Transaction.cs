using LiteDB;

namespace LH.Forcas.Domain.UserData
{
    using System;

    public class Transaction : IRoamingObject
    {
        [BsonId]
        public Guid TransactionId { get; set; }

        public string CounterpartyName { get; set; }

        public string CounterpartyAccountNumber { get; set; }

        public string Memo { get; set; }

        public string VariableSymbol { get; set; }

        public string ConstantSymbol { get; set; }

        public string SpecificSymbol { get; set; }

        public TransactionType TransactionType { get; set; }

        public BsonValue GetIdAsBson()
        {
            return new BsonValue(this.TransactionId);
        }
    }
}