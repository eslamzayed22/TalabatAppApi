using AutoMapper;
using DomainLayer.Models.ProductModels;
using Microsoft.Extensions.Configuration;
using Shared.DataTransferObjects.ProductDtos;

namespace ServiceLayer.MappingProfiles
{
    public class PictureUrlResolver(IConfiguration _configuration) : IValueResolver<Product, ProductDto, string>
    {
        public string Resolve(Product source, ProductDto destination, string destMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source.PictureUrl)) return string.Empty;
            else
            {
                return $"{_configuration.GetSection("Urls")["BaseUrl"]}{source.PictureUrl}";
            }
        }
    }
}