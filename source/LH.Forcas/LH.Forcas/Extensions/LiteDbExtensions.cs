using System.Collections.Generic;
using System.Linq;
using LH.Forcas.Domain.UserData;
using LiteDB;

namespace LH.Forcas.Extensions
{
    public static class LiteDbExtensions
    {
        public static LiteCollection<T> GetCollection<T>(this LiteDatabase database)
        {
            return database.GetCollection<T>(typeof(T).Name);
        }

        public static List<T> Fetch<T>(this LiteDatabase database)
        {
            return database.GetCollection<T>().FindAll().ToList();
        }

        public static T SingleOrDefault<T>(this LiteDatabase database)
        {
            return database.GetCollection<T>().FindOne(x => true);
        }

        public static T SingleOrDefault<T>(this LiteDatabase database, BsonValue id)
        {
            var collection = database.GetCollection<T>();
            var item = collection.FindById(id);

            return item;
            //return database.GetCollection<T>().FindById(id);
        }

        public static void Delete<T>(this LiteDatabase database, T item) where T : IUserEntity
        {
            database.GetCollection<T>().Delete(item.GetIdAsBson());
        }

        public static void DeleteAll<T>(this LiteDatabase database)
        {
            database.DropCollection(typeof(T).Name);
        }
    }
}