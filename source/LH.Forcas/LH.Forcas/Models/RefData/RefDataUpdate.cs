using System;

namespace LH.Forcas.Models.RefData
{
    public class RefDataUpdate<T> : RefDataUpdateBase
    {
        public T[] Data { get; set; }
    }

    public abstract class RefDataUpdateBase
    {
        public Type EntityType { get; set; }

        public int Version { get; set; }
    }
}