using DomainLayer.Contracts;
using DomainLayer.Models.ProductModels;
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

        public DataSeeding(StoreDbContext storeDbContext)
        {
            _storeDbContext = storeDbContext;
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
    }
}
