using System;

namespace LH.Forcas.Domain.RefData
{
    public class Bank
    {
        public Guid BankId { get; set; }

        public string Name { get; set; }

        public string IbanFormat { get; set; }

        public string CountryCode { get; set; }

        public int RoutingCode { get; set; }

        // TODO: Logo as byte[] ?
    }
}