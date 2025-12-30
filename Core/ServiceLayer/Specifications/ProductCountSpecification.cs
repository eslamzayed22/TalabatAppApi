using DomainLayer.Models.ProductModels;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Specifications
{
    public class ProductCountSpecification: BaseSpecifications<Product, int>
    {
        public ProductCountSpecification(ProductQueryParameter queryParameter) : base
            (p => (!queryParameter.BrandId.HasValue || p.BrandId == queryParameter.BrandId) &&
            (!queryParameter.TypeId.HasValue || p.TypeId == queryParameter.TypeId) &&
            (string.IsNullOrWhiteSpace(queryParameter.Search) || p.Name.ToLower().Contains(queryParameter.Search.ToLower())))
        {
            
        }
    }
}
