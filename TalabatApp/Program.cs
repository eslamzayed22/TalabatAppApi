
using DomainLayer.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PersistenceLayer;
using PersistenceLayer.Data;
using PersistenceLayer.Repositories;
using ServiceAbstractionLayer;
using ServiceLayer;
using ServiceLayer.MappingProfiles;
using Shared.ErrorModels;
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
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddSwaggerServices();
            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddWebAppServices();

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
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthorization();

            app.MapControllers(); 

            #endregion

            app.Run();
        }
    }
}
