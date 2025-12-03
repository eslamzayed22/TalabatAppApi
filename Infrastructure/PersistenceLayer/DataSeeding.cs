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
        public void DataSeed()
        {
            try
            {
                //Ensure that DB is already Migrated
                if (_storeDbContext.Database.GetPendingMigrations().Any())
                {
                    _storeDbContext.Database.Migrate();
                }

                if (!_storeDbContext.ProductBrands.Any())
                {
                    var ProductBrandData = File.ReadAllText(@"..\Infrastructure\PersistenceLayer\DataSeed\brands.json");
                    //var ProductBrandData = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(),
                    //                        "Infrastructure", "PersistenceLayer", "DataSeed", "brands.json"));

                    var ProductBrandObjs = JsonSerializer.Deserialize<List<ProductBrand>>(ProductBrandData);
                    if (ProductBrandObjs is not null && ProductBrandObjs.Any())
                    {
                        _storeDbContext.ProductBrands.AddRange(ProductBrandObjs);
                    }
                }

                if (!_storeDbContext.ProductTypes.Any())
                {
                    var ProductTypeData = File.ReadAllText(@"..\Infrastructure\PersistenceLayer\DataSeed\types.json");

                    var ProductTypeObjs = JsonSerializer.Deserialize<List<ProductType>>(ProductTypeData);
                    if (ProductTypeObjs is not null && ProductTypeObjs.Any())
                    {
                        _storeDbContext.ProductTypes.AddRange(ProductTypeObjs);
                    }
                }

                if (!_storeDbContext.Products.Any())
                {
                    var ProductData = File.ReadAllText(@"..\Infrastructure\PersistenceLayer\DataSeed\products.json");

                    var ProductsObjs = JsonSerializer.Deserialize<List<Product>>(ProductData);
                    if (ProductsObjs is not null && ProductsObjs.Any())
                    {
                        _storeDbContext.Products.AddRange(ProductsObjs);
                    }
                }

                _storeDbContext.SaveChanges();
            }
            catch (Exception)
            {

            }

        }
    }
}
