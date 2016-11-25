using System;
using System.Threading.Tasks;
using LH.Forcas.Contract;

namespace LH.Forcas.Integration
{
    public class DropBoxFileSyncProvider : IFileSyncProvider
    {
        public async Task UploadDeltaFileAsync(object contents, DateTime timestamp)
        {
            throw new NotImplementedException();
        }

        public async Task<object> DownloadDeltaFilesAfterTimestampAsync(DateTime timestamp)
        {
            throw new NotImplementedException();
        }
    }
}