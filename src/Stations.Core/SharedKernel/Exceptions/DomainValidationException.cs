using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace Stations.Core.SharedKernel.Exceptions
{
    public class DomainValidationException : Exception
    {
        public DomainValidationException()
        {
            
        }
        public DomainValidationException(ValidationResult results)
        {
            Errors = results.Errors.ToDictionary(x => x.PropertyName, x => x.ErrorMessage);
        }


        public Dictionary<string, string> Errors { get; set; }
    }
}
