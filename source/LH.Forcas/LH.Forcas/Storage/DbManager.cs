using System;
using LH.Forcas.Domain.UserData;
using LiteDB;

namespace LH.Forcas.Storage
{
    public class DbManager : IDbManager
    {
        public static void RegisterBsonMappings()
        {
            BsonMapper.Global.RegisterType(
                number => number.Iban,
                iban => AccountNumber.FromIban(iban));

            BsonMapper.Global.RegisterType(
                guid => guid.ToString("N"),
                str => Guid.Parse(str));
        }

        public DbManager(IPathResolver pathResolver)
        {
            RegisterBsonMappings();
            this.LiteRepository = new LiteRepository(pathResolver.DbFilePath);
        }

        public LiteRepository LiteRepository { get; }

        public void ApplyMigrations()
        {
            // Schema conversions to be done here
        }

        public void Dispose()
        {
            this.LiteRepository.Dispose();
        }
    }
}