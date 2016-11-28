using System;
using System.Reflection;
using System.Threading.Tasks;
using SQLite;

namespace LH.Forcas.Extensions
{
    public static class SqLiteExtensions
    {
        public static async Task<bool> TableExistsAsync<T>(this SQLiteAsyncConnection connection)
        {
            var tableName = typeof(T).GetTableName();
            var result = await connection.ExecuteScalarAsync<int?>("SELECT count(*) FROM sqlite_master WHERE type = 'table' AND name = ?", tableName);

            return result != null && (int)result > 0;
        }

        public static async Task<bool> CreateTableIfNotExistsAsync<T>(this SQLiteAsyncConnection connection) where T : class, new()
        {
            var exists = await connection.TableExistsAsync<T>();

            if (exists)
            {
                return false;
            }

            await connection.CreateTableAsync<T>();
            return true;
        }

        public static string GetTableName(this Type type)
        {
            var attribute = type.GetTypeInfo().GetCustomAttribute<TableAttribute>();

            if (attribute != null)
            {
                return attribute.Name;
            }

            return type.Name;
        }
    }
}