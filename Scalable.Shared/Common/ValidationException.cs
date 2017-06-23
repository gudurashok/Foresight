using System;

namespace Scalable.Shared.Common
{
    public class ValidationException : ApplicationException
    {
        public string PropertyName { get; private set; }

        public ValidationException(string message)
            : base(message)
        {
        }

        public ValidationException(string message, string propertyName)
            : base(message)
        {
            PropertyName = propertyName;
        }

        public ValidationException(ValidationResult result)
            : this(result.ToString())
        {
        }
    }
}
