using System;
using System.Collections.Generic;
using LH.Forcas.Storage.Entities.RefData;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace LH.Forcas.Storage.Entities.UserData
{
    public class AccountEntity
    {
        [PrimaryKey]
        public Guid AccountId { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<TransactionEntity> Transactions { get; set; }

        [ForeignKey(typeof(BankEntity))]
        public Guid BankId { get; set; }

        [ManyToOne]
        public BankEntity Bank { get; set; }

        [ForeignKey(typeof(CurrencyEntity))]
        public string CurrencyCode { get; set; }

        [ManyToOne]
        public CurrencyEntity Currency { get; set; }

        public decimal CurrentBalance { get; set; }
    }
}