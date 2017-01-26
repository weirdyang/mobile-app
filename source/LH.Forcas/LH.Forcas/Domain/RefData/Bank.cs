namespace LH.Forcas.Domain.RefData
{
    public class Bank : IIsActive
    {
        public string BankId { get; set; }

        public string Name { get; set; }

        public string IbanPrefix { get; set; }

        public string CountryId { get; set; }

        public int RoutingCode { get; set; }

        public BankAuthorizationScheme AuthorizationScheme { get; set; }

        public bool IsActive { get; set; }
    }
}