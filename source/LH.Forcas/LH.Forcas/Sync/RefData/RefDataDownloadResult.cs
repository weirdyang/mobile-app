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
            this.CommitSha = commitSha;
            this.Version = version;
        }

        public bool NewDataAvailable { get; }

        public bool NewIncompatibleDataAvailable { get; }

        public string CommitSha { get; }

        public int Version { get; }

        public RefDataUpdate Data { get; }
    }
}