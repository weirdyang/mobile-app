using System.Collections.Generic;

namespace LH.Forcas.Contract
{
    public interface IApp
    {
        IAppConstants Constants { get; }

        IDictionary<string, object> Properties { get; }
    }
}