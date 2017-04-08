using System.Threading.Tasks;

namespace LH.Forcas.Sync.RefData
{
    public interface IRefDataDownloader
    {
        Task<RefDataDownloadResult> DownloadRefData(string lastSyncedCommit, int lastSyncedVersion);
    }
}