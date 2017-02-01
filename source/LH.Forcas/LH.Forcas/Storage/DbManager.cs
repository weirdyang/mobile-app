using LiteDB;

namespace LH.Forcas.Storage
{
    using System;
    using Domain.UserData;

    public class DbManager : IDbManager
    {
        private readonly IPathResolver pathResolver;

        public DbManager(IPathResolver pathResolver)
        {
            this.pathResolver = pathResolver;
        }

        public virtual LiteDatabase GetDatabase()
        {
            return new LiteDatabase(this.pathResolver.DbFilePath);
        }

        public void Initialize()
        {
            // Schema conversions to be done here

            this.RegisterBsonMappings();
        }

        private void RegisterBsonMappings()
        {
            BsonMapper.Global.RegisterType(
                number => number.Iban,
                iban => AccountNumber.FromIban(iban));

            BsonMapper.Global.RegisterType(
                guid => guid.ToString("N"),
                str => Guid.Parse(str));
        }
    }
}