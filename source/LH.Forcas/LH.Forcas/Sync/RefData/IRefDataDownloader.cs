using System.Threading.Tasks;
using LH.Forcas.Domain.RefData;

namespace LH.Forcas.Sync.RefData
{
    public interface IRefDataDownloader
    {
        Task<RefDataDownloadResult> DownloadRefData(RefDataStatus status);
    }
}