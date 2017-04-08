using LH.Forcas.Domain.RefData;

namespace LH.Forcas.Sync.RefData
{
    public class RefDataDownloadResult
    {
        public RefDataDownloadResult(bool newDataAvailable = false, bool newIncompatibleDataAvailable = false)
        {
            this.NewDataAvailable = newDataAvailable;
            this.NewIncompatibleDataAvailable = newIncompatibleDataAvailable;
        }

        public RefDataDownloadResult(RefDataUpdate data, string commitSha, int version)
        {
            this.NewDataAvailable = true;
            this.Data = data;

            this.NewStatus = new RefDataStatus(commitSha, version);
        }

        public bool NewDataAvailable { get; }

        public bool NewIncompatibleDataAvailable { get; }

        public RefDataUpdate Data { get; }

        public RefDataStatus NewStatus { get; }
    }
}