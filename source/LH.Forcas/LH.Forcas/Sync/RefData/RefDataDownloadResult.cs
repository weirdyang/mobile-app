using LH.Forcas.Domain.RefData;
using LH.Forcas.RefDataContract;

namespace LH.Forcas.Sync.RefData
{
    public class RefDataDownloadResult
    {
        public RefDataDownloadResult(bool newDataAvailable = false)
        {
            this.NewDataAvailable = newDataAvailable;
        }

        public RefDataDownloadResult(RefDataUpdate data, string commitSha, int version)
        {
            this.NewDataAvailable = true;
            this.Data = data;

            this.NewStatus = new RefDataStatus(commitSha, version);
        }

        public bool NewDataAvailable { get; }

        public RefDataUpdate Data { get; }

        public RefDataStatus NewStatus { get; }
    }
}