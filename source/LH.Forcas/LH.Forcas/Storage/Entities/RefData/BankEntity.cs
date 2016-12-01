using System;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace LH.Forcas.Storage.Entities.RefData
{
    public class BankEntity
    {
        [PrimaryKey]
        public Guid BankId { get; set; }

        public string Name { get; set; }

        public string IbanFormat { get; set; }

        [ForeignKey(typeof(CountryEntity))]
        public string CountryCode { get; set; }

        [ManyToOne]
        public CountryEntity Country { get; set; }

        public short RoutingCode { get; set; }
    }
}