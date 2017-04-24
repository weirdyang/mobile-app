using LH.Forcas.Domain.UserData;

namespace LH.Forcas.Banking
{
    public class RemoteAccountInfo
    {
        public string CurrencyId { get; set; }

        public AccountNumber AccountNumber { get; set; }

        // TODO: BankProduct type?
    }
}