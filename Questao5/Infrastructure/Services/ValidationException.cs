using Questao5.Domain.Enumerators;
using System;

namespace Questao5.Infrastructure.Services
{
    public class ValidationException : Exception
    {
        public ValidationErrorType ErrorType { get; }

        public ValidationException(ValidationErrorType errorType) : base(errorType.ToString())
        {
            ErrorType = errorType;
        }
    }
}