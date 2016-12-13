using System;
using System.Linq;
using Newtonsoft.Json;

namespace LH.Forcas.Domain.RefData
{
    public class RefDataUpdate<T> : IRefDataUpdate
    {
        public T[] TypedData { get; set; }

        public Type Type => typeof(T);

        public string TypeName => typeof(T).Name;

        public int Version { get; set; }

        [JsonIgnore]
        public object[] Data => this.TypedData.Cast<object>().ToArray();
    }
}