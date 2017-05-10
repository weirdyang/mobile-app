using System;

namespace LH.Forcas
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property)]
    public class ExcludeFromCodeCoverageAttribute : Attribute
    {
    }
}