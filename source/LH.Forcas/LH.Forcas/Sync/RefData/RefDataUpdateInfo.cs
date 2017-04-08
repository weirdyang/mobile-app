using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LH.Forcas.Sync.RefData
{
    public class RefDataUpdateInfo
    {
        public int Version { get; set; }

        [JsonConverter(typeof(VersionConverter))]
        public Version MinAppVersion { get; set; }
    }
}