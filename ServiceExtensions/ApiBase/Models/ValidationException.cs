using System;
using System.Collections.Generic;

namespace ApiBase.Models
{
    /// <summary>
    /// customized validation exception derived from system.exception
    /// </summary>
    public class ValidationException : Exception
    {
        public Dictionary<string,string> ValidationErrors { get; set; }

        public ValidationException(string message):base(message)
        { 
        }

        public ValidationException(Dictionary<string, string> errors,string summary="Validation Errors") : base(summary)
        {
            ValidationErrors = errors;
        }
    }
}
