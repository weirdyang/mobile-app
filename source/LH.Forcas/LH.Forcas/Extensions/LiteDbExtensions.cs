using LiteDB;

namespace LH.Forcas.Extensions
{
    public static class LiteDbExtensions
    {
        public static void DeleteAll<T>(this LiteRepository repository)
        {
            repository.Database.DropCollection(typeof(T).Name);
        }
    }
}