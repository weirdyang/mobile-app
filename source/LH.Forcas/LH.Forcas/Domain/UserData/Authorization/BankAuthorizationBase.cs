namespace LH.Forcas.Domain.UserData.Authorization
{
    using System;
    using LiteDB;

    public abstract class BankAuthorizationBase : IRoamingObject
    {
        public Guid BankAuthId { get; set; }

        public string BankId { get; set; }

        public BsonValue GetIdAsBson()
        {
            return new BsonValue(this.BankAuthId);
        }
    }
}