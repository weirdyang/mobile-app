namespace LH.Forcas.ViewModels
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using FluentValidation.Results;

    public class ValidationResults
    {
        private readonly IDictionary<string, ValidationResult> validationResults;

        public ValidationResults()
        {
            this.validationResults = new Dictionary<string, ValidationResult>();
        }

        [IndexerName("MyItem")]
        public ValidationResult this[string propertyName]
        {
            get
            {
                ValidationResult result;
                if (this.validationResults.TryGetValue(propertyName, out result))
                {
                    return result;
                }

                return null;
            }
        }

        public void PushResults(string propertyName, ValidationResult newResults)
        {
            this.validationResults[propertyName] = newResults;
        }
    }
}