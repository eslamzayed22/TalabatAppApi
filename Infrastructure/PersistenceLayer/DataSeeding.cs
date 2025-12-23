using DomainLayer.Contracts;
using DomainLayer.Models.IdentityModels;
using DomainLayer.Models.ProductModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersistenceLayer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PersistenceLayer
{
    public class DataSeeding : IDataSeeding
    {
        private readonly StoreDbContext _storeDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DataSeeding(StoreDbContext storeDbContext, 
                            UserManager<ApplicationUser> userManager,
                            RoleManager<IdentityRole> roleManager)
        {
            _storeDbContext = storeDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task DataSeedAsync()
        {
            try
            {
                //Ensure that DB is already Migrated
                if ((await _storeDbContext.Database.GetPendingMigrationsAsync()).Any())
                {
                    await _storeDbContext.Database.MigrateAsync();
                }

                if (!(await _storeDbContext.ProductBrands.AnyAsync()))
                {
                    var ProductBrandData = File.OpenRead(@"..\Infrastructure\PersistenceLayer\DataSeed\brands.json");
                    //var ProductBrandData = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(),
                    //                        "Infrastructure", "PersistenceLayer", "DataSeed", "brands.json"));

                    var ProductBrandObjs = await JsonSerializer.DeserializeAsync<List<ProductBrand>>(ProductBrandData);
                    if (ProductBrandObjs is not null && ProductBrandObjs.Any())
                    {
                       await _storeDbContext.ProductBrands.AddRangeAsync(ProductBrandObjs);
                    }
                }

                if (!(await _storeDbContext.ProductTypes.AnyAsync()))
                {
                    var ProductTypeData = File.OpenRead(@"..\Infrastructure\PersistenceLayer\DataSeed\types.json");

                    var ProductTypeObjs = await JsonSerializer.DeserializeAsync<List<ProductType>>(ProductTypeData);
                    if (ProductTypeObjs is not null && ProductTypeObjs.Any())
                    {
                       await _storeDbContext.ProductTypes.AddRangeAsync(ProductTypeObjs);
                    }
                }

                if (!(await _storeDbContext.Products.AnyAsync()))
                {
                    var ProductData = File.OpenRead(@"..\Infrastructure\PersistenceLayer\DataSeed\products.json");

                    var ProductsObjs = await JsonSerializer.DeserializeAsync<List<Product>>(ProductData);
                    if (ProductsObjs is not null && ProductsObjs.Any())
                    {
                        await _storeDbContext.Products.AddRangeAsync(ProductsObjs);
                    }
                }

                await _storeDbContext.SaveChangesAsync();
            }
            catch (Exception)
            {

            }
        }

        public async Task IdentityDataSeedAsync()
        {
            try
            {
                if (!_roleManager.Roles.Any())
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                }
                if (!_userManager.Users.Any())
                {
                    var admin = new ApplicationUser
                    {
                        Email = "eslamzayed765@gmail.com",
                        DisplayName = "Eslam Zayed",
                        PhoneNumber = "01092059096",
                        UserName = "eslamzayed22"
                    };
                    var superAdmin = new ApplicationUser
                    {
                        Email = "ahmedzayed765@gmail.com",
                        DisplayName = "Ahmed Zayed",
                        PhoneNumber = "01092059096",
                        UserName = "aehmedzayed22"
                    };

                    await _userManager.CreateAsync(admin, "P@ssw0rd");
                    await _userManager.CreateAsync(superAdmin, "P@ssw0rd");

                    await _userManager.AddToRoleAsync(admin, "Admin");
                    await _userManager.AddToRoleAsync(superAdmin, "SuperAdmin");

                }
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
