namespace LH.Forcas.Domain.UserData.Authorization
{
    using System;
    using LiteDB;

    public abstract class BankAuthorizationBase : UserEntityBase<Guid>
    {
        public string BankId { get; set; }

        public override BsonValue GetIdAsBson()
        {
            return new BsonValue(this.Id);
        }
    }
}