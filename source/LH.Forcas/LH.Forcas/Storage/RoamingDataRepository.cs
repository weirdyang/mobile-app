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

        public void Insert<T>(T item)
        {
            using (var db = this.dbManager.GetDatabase())
            {
                db.GetCollection<T>().Insert(item);
            }
        }

        public void Update<T>(T item)
        {
            using (var db = this.dbManager.GetDatabase())
            {
                db.GetCollection<T>().Update(item);
            }
        }

        public IRepositoryTransaction BeginTransaction()
        {
            return new RoamingRepositoryTransaction(this.dbManager.GetDatabase());
        }
    }
}