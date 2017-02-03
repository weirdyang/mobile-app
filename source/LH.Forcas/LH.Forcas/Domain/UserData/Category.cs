namespace LH.Forcas.Domain.UserData
{
    using System;
    using System.Collections.Generic;
    using LiteDB;

    public class Category : IRoamingObject
    {
        [BsonId]
        public Guid CategoryId { get; set; }

        public string Icon { get; set; }

        public string Name { get; set; }

        public List<Category> Children { get; set; }

        public BsonValue GetIdAsBson()
        {
            return new BsonValue(this.CategoryId);
        }
    }
}