namespace LH.Forcas.Domain.UserData
{
    using System;
    using LiteDB;

    public abstract class Account : IRoamingObject
    {
        public Guid AccountId { get; set; }

        public string Name { get; set; }

        public string CurrencyId { get; set; }

        public Amount CurrentBalance { get; set; }

        public DateTime LastSyncUtcTime { get; set; }

        public BsonValue GetIdAsBson()
        {
            return new BsonValue(this.AccountId);
        }
    }

    public abstract class BankProductAccount : Account
    {
        public string BankId { get; set; }
    }

    public class BankAccount : BankProductAccount
    { 
        public AccountNumber AccountNumber { get; set; }
    }

    public class CreditCardAccount : BankProductAccount
    {
        public string CardNumber { get; set; }
    }

    public class CashAccount : Account
    {

    }

    public class InvestmentAccount : Account
    {
        
    }

    public class LoanAccount : Account
    {
        
    }
}