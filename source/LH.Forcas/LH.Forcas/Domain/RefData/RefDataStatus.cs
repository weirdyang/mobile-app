using LiteDB;

namespace LH.Forcas.Domain.RefData
{
    public class RefDataStatus
    {
        public const string SingleId = "RefDataStatus";

        public RefDataStatus()
        {
        }

        public RefDataStatus(string commitSha, int dataVersion)
        {
            this.CommitSha = commitSha;
            this.DataVersion = dataVersion;
        }

        [BsonId]
        public string Id => SingleId;

        public int DataVersion { get; set; }

        public string CommitSha { get; set; }
    }
}