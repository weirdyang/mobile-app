namespace LH.Forcas.Domain.UserData
{
    using System;
    using System.Collections.Generic;
    using LiteDB;

    public abstract class Account
    {
        [BsonId]
        public Guid AccountId { get; set; }

        public string Name { get; set; }

        public string CurrencyId { get; set; }

        public decimal CurrentBalance { get; set; }
    }

    public class CashAccount : Account
    {
        
    }

    public class BankAccount : Account
    {
        public string BankId { get; set; }

        public BankAccountType Type { get; set; }

        public AccountNumber AccountNumber { get; set; }
    }

    public class InvestmentAccount : Account
    {
        
    }

    public class LoanAccount : Account
    {
        
    }
}