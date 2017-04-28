using LiteDB;

namespace LH.Forcas.Domain.UserData
{
    public abstract class UserEntityBase<TId>
    {
        [BsonId]
        public virtual TId Id { get; set; }

        public abstract BsonValue GetIdAsBson();
    }
}