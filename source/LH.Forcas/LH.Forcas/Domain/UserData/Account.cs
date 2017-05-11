namespace LH.Forcas.Domain.UserData
{
    using System;
    using LiteDB;

    public abstract class Account : UserEntityBase<Guid>, ISoftDeletable
    {
        public string Name { get; set; }

        public string CurrencyId { get; set; }

        public Amount CurrentBalance { get; set; }

        public DateTime LastSyncUtcTime { get; set; }

        public bool IsDeleted { get; set; }

        public override BsonValue GetIdAsBson()
        {
            return new BsonValue(this.Id);
        }
    }

    public abstract class BankAccount : Account
    {
        public string BankId { get; set; }

        public AccountNumber AccountNumber { get; set; }
    }

    public class CheckingAccount : BankAccount
    {
    }

    public class SavingsAccount : BankAccount
    {
    }

    public class CreditCardAccount : LoanAccount
    {
        public string CardNumber { get; set; }
    }

    public class CashAccount : Account
    {
    }

    public class InvestmentAccount : Account
    {
    }

    public abstract class LoanAccount : Account
    {
        
    }
}