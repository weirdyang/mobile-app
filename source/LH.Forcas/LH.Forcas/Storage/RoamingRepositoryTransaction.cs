namespace LH.Forcas.Storage
{
    using System.Collections.Generic;
    using Extensions;
    using LiteDB;

    public class RoamingRepositoryTransaction : IRepositoryTransaction
    {
        private readonly LiteDatabase db;
        private readonly LiteTransaction transaction;

        public RoamingRepositoryTransaction(LiteDatabase db)
        {
            this.db = db;
            this.transaction = db.BeginTrans();
        }

        public IEnumerable<T> GetAll<T>()
        {
            return this.db.GetCollection<T>().FindAll();
        }

        public T GetOneById<T>(object id)
        {
            var bsonId = new BsonValue(id);
            return this.db.GetCollection<T>().FindById(bsonId);
        }

        public void Delete<T>(object id)
        {
            var bsonId = new BsonValue(id);
            this.db.GetCollection<T>().Delete(bsonId);
        }

        public void Save<T>(T item)
        {
            this.db.GetCollection<T>().Upsert(item);
        }

        public void Dispose()
        {
            this.transaction.Dispose();
            this.db.Dispose();
        }

        public void Complete()
        {
            this.transaction.Commit();
        }
    }
}