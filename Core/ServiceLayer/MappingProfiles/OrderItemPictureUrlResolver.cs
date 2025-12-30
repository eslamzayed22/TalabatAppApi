using AutoMapper;
using AutoMapper.Execution;
using DomainLayer.Models.OrderModels;
using Microsoft.Extensions.Configuration;
using Shared.DataTransferObjects.OrderDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.MappingProfiles
{
    public class OrderItemPictureUrlResolver(IConfiguration _configuration) : IValueResolver<OrderItem, OrderItemDto, string>
    {
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source.Product.PictureUrl)) return string.Empty;

            return $"{_configuration.GetSection("Urls")["BaseUrl"]}{source.Product.PictureUrl}";
        }
    }
}
