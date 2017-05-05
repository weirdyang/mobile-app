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
            return database.GetCollection<T>().FindById(id);
        }

        public static void Upsert<T>(this LiteDatabase database, T item)
        {
            database.GetCollection<T>().Upsert(item);
        }

        public static void Delete<T>(this LiteDatabase database, T item) where T : IUserEntity
        {
            database.GetCollection<T>().Delete(item.GetIdAsBson());
        }

        public static int DeleteAll<T>(this LiteDatabase database)
        {
            return database.GetCollection<T>().Delete(x => true);
        }
    }
}