namespace LH.Forcas.Domain.UserData
{
    using System;
    using LiteDB;

    public abstract class Account : IRoamingObject
    {
        public Guid AccountId { get; set; }

        public string Name { get; set; }

        public string CurrencyId { get; set; }

        public decimal CurrentBalance { get; set; }

        public BsonValue GetIdAsBson()
        {
            return new BsonValue(this.AccountId);
        }
    }

    public class CashAccount : Account
    {
        
    }

    public class BankAccount : Account
    {
        // TODO: Add some kind of product identification (to match the pricelists)

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