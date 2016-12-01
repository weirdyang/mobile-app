using System;
using SQLite.Net.Attributes;

namespace LH.Forcas.Storage.Entities.UserData
{
    public class CategoryEntity
    {
        [PrimaryKey]
        public Guid CategoryId { get; set; }

        public string Name { get; set; }
    }
}