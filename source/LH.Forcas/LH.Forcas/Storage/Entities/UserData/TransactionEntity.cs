using System;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace LH.Forcas.Storage.Entities.UserData
{
    public class TransactionEntity
    {
        [PrimaryKey]
        public Guid TransactionId { get; set; }

        [ForeignKey(typeof(AccountEntity))]
        public Guid AccountId { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.None)]
        public AccountEntity Account { get; set; }

        [ForeignKey(typeof(CategoryEntity))]
        public Guid CategoryId { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.None)]
        public CategoryEntity Category { get; set; }

        public string CounterpartyName { get; set; }

        public string CounterpartyAccountNumber { get; set; }

        public string Memo { get; set; }

        public string VariableSymbol { get; set; }

        public string ConstantSymbol { get; set; }

        public string SpecificSymbol { get; set; }

        public short TransactionType { get; set; }
    }
}
