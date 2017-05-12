using Newtonsoft.Json;

namespace LH.Forcas.ViewModels.About
{
    public class Dependency
    {
        public string Name { get; set; }

        public string Version { get; set; }

        [JsonIgnore]
        public string DisplayName => $"{this.Name} ({this.Version})";
    }
}