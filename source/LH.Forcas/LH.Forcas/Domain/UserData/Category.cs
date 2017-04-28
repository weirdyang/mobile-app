namespace LH.Forcas.Domain.UserData
{
    using System;
    using System.Collections.Generic;
    using LiteDB;

    public class Category : UserEntityBase<Guid>
    {
        public string Icon { get; set; }

        public string Name { get; set; }

        public List<Category> Children { get; set; }

        public override BsonValue GetIdAsBson()
        {
            return new BsonValue(this.Id);
        }
    }
}