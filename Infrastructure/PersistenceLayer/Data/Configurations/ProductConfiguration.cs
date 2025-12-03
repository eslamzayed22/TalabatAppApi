using DomainLayer.Models.ProductModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistenceLayer.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(nameof(Product.Name)).HasColumnType("varchar(50)");
            builder.Property(nameof(Product.Description)).HasColumnType("varchar(600)");
            builder.Property(nameof(Product.PictureUrl)).HasColumnType("varchar(500)");
            builder.Property(nameof(Product.Price)).HasColumnType("decimal(10,2)");

            builder.HasOne(p => p.ProductBrand)
                   .WithMany()
                   .HasForeignKey(p => p.BrandId);

            builder.HasOne(p => p.ProductType)
                     .WithMany()
                     .HasForeignKey(p => p.TypeId);

        }
    }
}
