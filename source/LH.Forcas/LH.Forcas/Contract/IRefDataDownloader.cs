using System.Threading.Tasks;
using LH.Forcas.Models;

namespace LH.Forcas.Contract
{
    public interface IRefDataDownloader
    {
        Task<ConfigData> GetUpdatedFiles();
    }
}