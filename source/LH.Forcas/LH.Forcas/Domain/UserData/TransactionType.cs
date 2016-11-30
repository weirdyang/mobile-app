namespace LH.Forcas.Domain.UserData
{
    public enum TransactionType : short
    {
        Unknown = 0,
        WireTransfer = 1,
        CashWithdrawal = 2,
        CashDeposit = 3,
        CardPayment = 4,
        BankFee = 5
    }
}