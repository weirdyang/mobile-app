using LiteDB;

namespace LH.Forcas.Extensions
{
    public static class LiteDbExtensions
    {
        public static void DeleteAll<T>(this LiteRepository repository)
        {
            repository.Delete<T>(x => true);
        }
    }
}