namespace LH.Forcas.Domain.UserData
{
    using System;
    using LiteDB;

    public class Account
    {
        [BsonId]
        public Guid AccountId { get; set; }

        public string Name { get; set; }

        public string BankCode { get; set; }

        public string CurrencyCode { get; set; }

        public AccountType Type { get; set; }

        public AccountNumber AccountNumber { get; set; }

        public decimal CurrentBalance { get; set; }
    }
}