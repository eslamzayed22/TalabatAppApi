using Microsoft.AspNetCore.Mvc;
using TalabatApp.Factories;

namespace TalabatApp.Extentions
{
    public static class ServicesRegisteration
    {
        public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }

        public static IServiceCollection AddWebAppServices(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>((options) =>
            {
                options.InvalidModelStateResponseFactory = ApiResponseFactory.GenerateApiValidationErrorResponse;
            });
            return services;
        }
    }
}
