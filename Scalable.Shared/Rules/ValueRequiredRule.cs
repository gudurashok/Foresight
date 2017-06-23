using Scalable.Shared.Common;

namespace Scalable.Shared.Rules
{
    public class ValueRequiredRule : IValidator
    {
        private readonly string _fieldName;
        private readonly string _value;

        public ValueRequiredRule(string fieldName, string value)
        {
            _fieldName = fieldName;
            _value = value;
        }

        public ValidationResult Validate()
        {
            var result = new ValidationResult();
            if (string.IsNullOrEmpty(_value))
                result.AddError(_fieldName + " Required");

            return result;
        }
    }
}
