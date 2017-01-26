namespace LH.Forcas.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using FluentValidation.Results;

    public class ValidationResults
    {
        private readonly IDictionary<string, PropertyValidationResult> errors;

        public ValidationResults(ValidationResult result)
        {
            this.IsValid = result.IsValid;

            this.errors = result.Errors
                                .GroupBy(x => x.PropertyName)
                                .ToDictionary(
                                    x => x.Key, 
                                    x => new PropertyValidationResult(x.Select(err => err.ErrorMessage)));
        }

        public ValidationResults()
        {
            this.IsValid = true;
            this.errors = new Dictionary<string, PropertyValidationResult>();
        }

        public bool IsValid { get; private set; }

        public int ErrorsCount => this.errors.Count;

        public IEnumerable<string> Properties
        {
            get { return this.errors.Keys; }
        }

        public PropertyValidationResult this[string propertyName]
        {
            get
            {
                PropertyValidationResult result;
                if (this.errors.TryGetValue(propertyName, out result))
                {
                    return result;
                }

                return null;
            }
        }

        public void UpdateProperty(string propertyName, ValidationResult result)
        {
            if (!result.IsValid)
            {
                IEnumerable<string> errorMessages = null;
                if (result.Errors != null && result.Errors.Count > 0)
                {
                    errorMessages = result.Errors.Select(x => x.ErrorMessage);
                }

                var propertyResult = new PropertyValidationResult(errorMessages);

                this.errors[propertyName] = propertyResult;
            }
            else
            {
                this.errors.Remove(propertyName);
            }

            this.IsValid = this.errors.Count == 0;
        }
    }
}