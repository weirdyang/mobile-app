using System.Threading.Tasks;
using LH.Forcas.Models;

namespace LH.Forcas.Storage
{
    public interface IUserDataRepository
    {
        Task<UserPreferences> GetUserPreferencesAsync();
    }
}