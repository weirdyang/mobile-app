using LiteDB;

namespace LH.Forcas.Extensions
{
    public static class LiteDbExtensions
    {
        public static LiteCollection<T> GetCollection<T>(this LiteDatabase db)
        {
            return db.GetCollection<T>(typeof(T).Name);
        }
        public static void Upsert<T>(this LiteCollection<T> collection, T item)
        {
            if (!collection.Update(item))
            {
                collection.Insert(item);
            }
        }

        public static void Upsert<T>(this LiteCollection<T> collection, T item, bool shouldInsert)
        {
            if (shouldInsert)
            {
                collection.Insert(item);
            }
            else
            {
                collection.Update(item);
            }
        }
    }
}