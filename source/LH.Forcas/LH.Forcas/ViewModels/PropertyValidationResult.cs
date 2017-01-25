namespace LH.Forcas.ViewModels
{
    using System.Collections.Generic;

    public class PropertyValidationResult
    {
        public PropertyValidationResult(IEnumerable<string> errorMessages)
        {
            if (errorMessages != null)
            {
                this.ErrorMessage = string.Join(" ", errorMessages);
            }

            this.IsValid = string.IsNullOrEmpty(this.ErrorMessage);
        }

        public bool IsValid { get; private set; }

        public string ErrorMessage { get; }
    }
}