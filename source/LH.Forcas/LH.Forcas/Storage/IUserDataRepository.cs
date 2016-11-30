using System.Threading.Tasks;
using LH.Forcas.Domain;

namespace LH.Forcas.Storage
{
    public interface IUserDataRepository
    {
        Task<UserPreferences> GetUserPreferencesAsync();
    }
}