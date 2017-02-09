namespace LH.Forcas.Domain.RefData
{
    public class Country : IIsActive
    {
        public string CountryId { get; set; }

        public string DefaultCurrencyId { get; set; }

        public bool IsActive { get; set; }
    }
}