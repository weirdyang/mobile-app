using System;

namespace LH.Forcas.Integration.Exceptions
{
    public class BankPayloadFormatException : Exception
    {
        public BankPayloadFormatException(string message, Exception inner = null)
            : base(message, inner)
        {
        }
    }
}