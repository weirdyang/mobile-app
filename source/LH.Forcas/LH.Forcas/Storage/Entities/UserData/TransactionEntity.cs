using System;
using SQLite.Net.Attributes;

namespace LH.Forcas.Storage.Entities.UserData
{
    public class TransactionEntity
    {
        [PrimaryKey]
        public Guid TransactionId { get; set; }

        public string CounterpartyName { get; set; }

        public string CounterpartyAccountNumber { get; set; }

        public string Memo { get; set; }

        public string VariableSymbol { get; set; }

        public string ConstantSymbol { get; set; }

        public string SpecificSymbol { get; set; }

        public short TransactionType { get; set; }
    }
}
