using System.Threading.Tasks;
using SQLite;

namespace LH.Forcas.Extensions
{
    public static class SqLiteExtensions
    {
        public static async Task<bool> TableExistsAsync<T>(this SQLiteAsyncConnection connection)
        {
            var result = await connection.ExecuteAsync("SELECT count(*) FROM sqlite_master WHERE type = 'table' AND name = ?", typeof(T).Name);

            return result > 0;
        }

        public static async Task CreateTableIfNotExistsAsync<T>(this SQLiteAsyncConnection connection) where T : class, new()
        {
            var exists = await connection.TableExistsAsync<T>();

            if (!exists)
            {
                await connection.CreateTableAsync<T>();
            }
        }
    }
}