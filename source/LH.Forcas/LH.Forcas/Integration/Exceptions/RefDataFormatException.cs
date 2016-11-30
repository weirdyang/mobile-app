using System;

namespace LH.Forcas.Integration.Exceptions
{
    public class RefDataFormatException : Exception
    {
        public RefDataFormatException(string message, Exception inner = null)
            : base(message, inner)
        {
            
        }
    }
}