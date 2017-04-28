using System;

namespace LH.Forcas.Banking.Exceptions
{
    public class BankPayloadFormatException : Exception
    {
        public BankPayloadFormatException(string message, Exception inner = null)
            : base(message, inner)
        {
        }
    }
}