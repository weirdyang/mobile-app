namespace LH.Forcas.Models.RefData
{
    public class Bank
    {
        public string Name { get; set; }

        public string IbanFormat { get; set; }

        public string CountryCode { get; set; }

        public int RountingCode { get; set; }

        // Note: Logo as byte[] ?
    }
}