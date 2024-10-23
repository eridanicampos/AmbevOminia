using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Domain.Helpers
{
    public abstract class BaseController : ControllerBase
    {
        protected ICollection<string> Erros = new List<string>();
        protected bool HasErros() => Erros.Any();

        protected ActionResult CustomResponse(object result = null)
        {
            if (!HasErros())
            {
                if (result == null)
                    return new NoContentResult();

                return new ObjectResult(result);
            }

            return new BadRequestObjectResult(new ValidationProblemDetails(new Dictionary<string, string[]>
        {
            { "Messages", Erros.ToArray() }
        }));
        }

        protected ActionResult CustomResponse(Stream result)
        {
            if (!HasErros())
            {
                if (result == null)
                    return new NoContentResult();

                return new FileStreamResult(result, "image/png");
            }

            return new BadRequestObjectResult(new ValidationProblemDetails(new Dictionary<string, string[]>
        {
            { "Messages", Erros.ToArray() }
        }));

        }


        protected ActionResult CustomResponse(ValidationException validationResult)
        {
            foreach (var error in validationResult.Errors)
                AddError($"{error.PropertyName} - {error.ErrorMessage}");

            return CustomResponse();
        }

        protected void AddError(string errorMessage) => Erros.Add(errorMessage);

        protected ActionResult CustomResponse<T>(BaseResponse<T> response) where T : class
        {
            return response.Success ? new ObjectResult(response) : CustomResponse(response.ValidationResult);
        }

    }
}
