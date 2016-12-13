using LiteDB;

namespace LH.Forcas.Storage
{
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
        }
    }
}