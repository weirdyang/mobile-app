using System.Collections.Generic;

namespace LH.Forcas
{
    public interface IApp
    {
        IAppConstants Constants { get; }

        IDictionary<string, object> Properties { get; }
    }
}