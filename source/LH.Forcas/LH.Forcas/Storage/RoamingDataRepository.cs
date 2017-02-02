namespace LH.Forcas.Storage
{
    using Extensions;
    using System.Collections.Generic;
    using System.Linq;
    using LiteDB;

    public class RoamingDataRepository : IRoamingDataRepository
    {
        private readonly IDbManager dbManager;

        public RoamingDataRepository(IDbManager dbManager)
        {
            this.dbManager = dbManager;
        }

        public IEnumerable<T> GetAll<T>()
        {
            using (var db = this.dbManager.GetDatabase())
            {
                return db.GetCollection<T>().FindAll().ToArray();
            }
        }

        public T GetOneById<T>(object id)
        {
            using (var db = this.dbManager.GetDatabase())
            {
                var bsonId = new BsonValue(id);
                return db.GetCollection<T>().FindById(bsonId);
            }
        }

        public void Delete<T>(object id)
        {
            using (var db = this.dbManager.GetDatabase())
            {
                var bsonId = new BsonValue(id);
                db.GetCollection<T>().Delete(bsonId);
            }
        }
    }
}