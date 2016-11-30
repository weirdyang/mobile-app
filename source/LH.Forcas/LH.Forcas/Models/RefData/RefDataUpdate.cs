using System;
using System.Linq;
using Newtonsoft.Json;

namespace LH.Forcas.Models.RefData
{
    public class RefDataUpdate<T> : IRefDataUpdate
    {
        public T[] TypedData { get; set; }

        public Type EntityType { get; set; }

        public int Version { get; set; }

        [JsonIgnore]
        public object[] Data => this.TypedData.Cast<object>().ToArray();
    }

    public interface IRefDataUpdate
    {
        Type EntityType { get; }

        int Version { get; }

        object[] Data { get; }
    }
}