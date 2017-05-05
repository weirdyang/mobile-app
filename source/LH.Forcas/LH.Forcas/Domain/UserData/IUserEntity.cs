using LiteDB;

namespace LH.Forcas.Domain.UserData
{
    public interface IUserEntity
    {
        BsonValue GetIdAsBson();
    }
}