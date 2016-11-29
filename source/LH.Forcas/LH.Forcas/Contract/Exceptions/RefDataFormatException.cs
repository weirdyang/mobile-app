using System;

namespace LH.Forcas.Contract.Exceptions
{
    public class RefDataFormatException : Exception
    {
        public RefDataFormatException(string message, Exception inner = null)
            : base(message, inner)
        {
            
        }
    }
}