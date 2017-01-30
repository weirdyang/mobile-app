namespace LH.Forcas.Integration.Banks
{
    using Domain.UserData;

    public class RemoteAccountInfo
    {
        public string CurrencyId { get; set; }

        public AccountNumber AccountNumber { get; set; }

        public BankAccountType Type { get; set; }
    }
}