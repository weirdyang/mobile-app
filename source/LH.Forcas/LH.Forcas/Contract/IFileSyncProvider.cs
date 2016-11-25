using System;
using System.Threading.Tasks;

namespace LH.Forcas.Contract
{
    public interface IFileSyncProvider
    {
        Task UploadDeltaFileAsync(object contents, DateTime timestamp);

        Task<object> DownloadDeltaFilesAfterTimestampAsync(DateTime timestamp);
    }
}