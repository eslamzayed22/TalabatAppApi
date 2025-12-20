using DomainLayer.Exceptions;
using Shared.ErrorModels;
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

                //Set status code for the response
                //httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                //Or
                //httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                ErrorToReturn response = HandelExceptionAsync(httpContext, ex);

                //Return Object as JSON
                await httpContext.Response.WriteAsJsonAsync(response);
            }
        }

        private static ErrorToReturn HandelExceptionAsync(HttpContext httpContext, Exception ex)
        {
            httpContext.Response.StatusCode = ex switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };

            //Response Object
            var response = new ErrorToReturn()
            {
                StatusCode = httpContext.Response.StatusCode,
                ErrorMessage = ex.Message
            };
            return response;
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
