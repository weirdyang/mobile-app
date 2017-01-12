namespace LH.Forcas.Domain.RefData
{
    public class Country : IIsActive
    {
        public string CountryCode { get; set; }

        public string DefaultCurrencyCode { get; set; }

        public bool IsActive { get; set; }
    }
}