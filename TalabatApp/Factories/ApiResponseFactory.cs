using Azure;
using Microsoft.AspNetCore.Mvc;
using Shared.ErrorModels;

namespace TalabatApp.Factories
{
    public static class ApiResponseFactory
    {
        public static IActionResult GenerateApiValidationErrorResponse(ActionContext context)
        {
            var errors = context.ModelState.Where(M => M.Value.Errors.Any())
            .Select(M => new ValidationError()
            {
                Field = M.Key,
                Errors = M.Value.Errors.Select(E => E.ErrorMessage)
            });
            var responese = new ValidationErrorToReturn()
            {
                ValidationErrors = errors
            };
            return new BadRequestObjectResult(responese);
        }
    };
}

