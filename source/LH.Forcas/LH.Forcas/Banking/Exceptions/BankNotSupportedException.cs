using System;

namespace LH.Forcas.Banking.Exceptions
{
    public class BankNotSupportedException : NotSupportedException
    {
        private const string MessageFormat = "The bank ID {0} is not supported for this operation.";

        public BankNotSupportedException(string bankId)
            : base(string.Format(MessageFormat, bankId))
        {
            this.BankId = bankId;
        }

        public BankNotSupportedException(string bankId, Exception inner)
            : base(string.Format(MessageFormat, bankId), inner)
        {
            this.BankId = bankId;
        }

        public string BankId { get; set; }
    }
}