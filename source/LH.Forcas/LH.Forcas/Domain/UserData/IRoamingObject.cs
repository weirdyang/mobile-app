namespace LH.Forcas.Domain.UserData
{
    using LiteDB;

    public interface IRoamingObject
    {
        BsonValue GetIdAsBson();
    }
}