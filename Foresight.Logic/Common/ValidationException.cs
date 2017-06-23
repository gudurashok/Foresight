using System;

namespace ScalableApps.Foresight.Logic.Common
{
    public class ValidationException : ApplicationException
    {
        public ValidationException(string message)
            : base(message)
        {
        }

        public ValidationException(ValidationResult result)
            : this(result.ToString())
        {
        }
    }
}
