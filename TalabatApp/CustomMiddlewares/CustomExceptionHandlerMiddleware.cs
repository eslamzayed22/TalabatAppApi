using DomainLayer.Exceptions;
using Shared.ErrorModels;
using System;
using System.Net;

namespace TalabatApp.CustomMiddlewares
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionHandlerMiddleware> _logger;

        public CustomExceptionHandlerMiddleware(RequestDelegate Next, ILogger<CustomExceptionHandlerMiddleware> logger)
        {
            _next = Next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next.Invoke(httpContext);
                await HandleNotFoundEndPointAsync(httpContext);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something Went Wrong");

               
                await HandelExceptionAsync(httpContext, ex);
            }
        }

        private static async Task HandelExceptionAsync(HttpContext httpContext, Exception ex)
        {
            //Response Object
            var response = new ErrorToReturn()
            {
                ErrorMessage = ex.Message
            };

            response.StatusCode = ex switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                UnauthorizedException => StatusCodes.Status401Unauthorized,
                BadRequestException badReqEx => GetBadRequestErrors(response, badReqEx),
                _ => StatusCodes.Status500InternalServerError
            };

            

            await httpContext.Response.WriteAsJsonAsync(response);
        }

        private static int GetBadRequestErrors(ErrorToReturn response, BadRequestException exception)
        {
            response.Errors = exception.Errors;
            return StatusCodes.Status400BadRequest;
        }

        private static async Task HandleNotFoundEndPointAsync(HttpContext httpContext)
        {
            if (httpContext.Response.StatusCode == StatusCodes.Status404NotFound)
            {
                var response = new ErrorToReturn()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    ErrorMessage = $"Endpoint {httpContext.Request.Path} is Not Found"
                };
                await httpContext.Response.WriteAsJsonAsync(response);
            }
        }

    }
}
