
using DomainLayer.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PersistenceLayer;
using PersistenceLayer.Data;
using PersistenceLayer.Repositories;
using ServiceAbstractionLayer;
using ServiceLayer;
using ServiceLayer.MappingProfiles;

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
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddScoped<IDataSeeding, DataSeeding>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IServiceManager, ServiceManager>();
            builder.Services.AddAutoMapper((x) => { }, typeof(ServiceAssemblyReference).Assembly);

            #endregion

            var app = builder.Build();

            #region Data Seeding

            using var scope = app.Services.CreateScope();
            var seedObj = scope.ServiceProvider.GetRequiredService<IDataSeeding>();

            await seedObj.DataSeedAsync();

            #endregion

            #region Configure the HTTP request pipeline.
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
