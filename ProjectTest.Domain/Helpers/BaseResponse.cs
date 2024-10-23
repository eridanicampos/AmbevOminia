using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Domain.Helpers
{
    public class BaseResponse<T> where T : notnull
    {
        public T Data { get; }

        public BaseResponse(T data) => Data = data;

        public ValidationResult ValidationResult { get; }

        public BaseResponse(ValidationResult validationResult) => ValidationResult = validationResult;

        public BaseResponse(T result, ValidationResult validationResult) : this(result) => ValidationResult = validationResult;

        public bool Success => ValidationResult?.IsValid ?? true;
    }
}
