using System.Threading.Tasks;
using SQLite;

namespace LH.Forcas.Contract
{
    public interface IDbManager
    {
        Task Initialize();

        SQLiteAsyncConnection GetConnection();
    }
}
