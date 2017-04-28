using System;
using LiteDB;

namespace LH.Forcas.Domain.UserData
{
    public class Transaction : UserEntityBase<Guid>
    { 
        public string CounterpartyName { get; set; }

        public string CounterpartyAccountNumber { get; set; }

        public string Memo { get; set; }

        public string VariableSymbol { get; set; }

        public string ConstantSymbol { get; set; }

        public string SpecificSymbol { get; set; }

        public TransactionType TransactionType { get; set; }

        public override BsonValue GetIdAsBson()
        {
            return new BsonValue(this.Id);
        }
    }
}