﻿using System.Collections.Generic;

namespace Scalable.Shared.Common
{
    public interface IValidator
    {
        ValidationResult Validate();
    }

    public class ValidationResult
    {
        private readonly List<string> _errorCodes = new List<string>();
        public void AddError(string errorCode)
        {
            _errorCodes.Add(errorCode);
        }

        public bool IsValid()
        {
            return _errorCodes.Count == 0;
        }

        public string[] GetErrors()
        {
            return _errorCodes.ToArray();
        }

        public override string ToString()
        {
            return _errorCodes[0];
        }
    }
}
