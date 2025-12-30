
using DomainLayer.Contracts;
using DomainLayer.Models.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PersistenceLayer;
using PersistenceLayer.Data;
using PersistenceLayer.Data.Identity;
using PersistenceLayer.Repositories;
using ServiceAbstractionLayer;
using ServiceLayer;
using ServiceLayer.MappingProfiles;
using Shared.ErrorModels;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Globalization;
using TalabatApp.CustomMiddlewares;
using TalabatApp.Extentions;
using TalabatApp.Factories;

namespace TalabatApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Add services to the container.
            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.AllowAnyOrigin();
                });
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddSwaggerServices();
            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddWebAppServices();
            builder.Services.AddJWTServices(builder.Configuration);


            #endregion

            var app = builder.Build();

            #region Data Seeding
            await app.SeedDatabaseAsync();

            #endregion

            #region Configure the HTTP request pipeline.

            app.UseCustomExceptionMiddleware();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.ConfigObject = new Swashbuckle.AspNetCore.SwaggerUI.ConfigObject()
                    {
                        DisplayRequestDuration = true
                    };
                    options.DocumentTitle = "Talabat API Project";
                    options.DocExpansion(DocExpansion.None);
                    options.EnableFilter();
                    options.EnablePersistAuthorization();
                });
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors("AllowAll");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers(); 

            #endregion

            app.Run();
        }
    }
}
