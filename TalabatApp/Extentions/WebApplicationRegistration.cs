using DomainLayer.Contracts;
using TalabatApp.CustomMiddlewares;

namespace TalabatApp.Extentions
{
    public static class WebApplicationRegistration
    {
        public async static Task SeedDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var seedObj = scope.ServiceProvider.GetRequiredService<IDataSeeding>();

            await seedObj.DataSeedAsync();
        }

        public static IApplicationBuilder UseCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<CustomExceptionHandlerMiddleware>();
            return app;

        }
    }
}
