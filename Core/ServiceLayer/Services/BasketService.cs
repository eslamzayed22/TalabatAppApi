using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Exceptions;
using DomainLayer.Models.BasketModels;
using ServiceAbstractionLayer;
using Shared.DataTransferObjects.BasketDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public class BasketService(IBasketRepository _basketRepository, IMapper _mapper) : IBasketService
    {
        public async Task<BasketDto> CreateOrUpdateBasketAsync(BasketDto basketDto)
        {
            var CustomerBasket = _mapper.Map<CustomerBasket>(basketDto);
            var createOrUpdateBasket = await _basketRepository.CreateOrUpdateBasketAsync(CustomerBasket);
            if (createOrUpdateBasket is not null) return await GetBasketAsync(basketDto.Id);
            else
            {
                throw new Exception("Failed to create or update basket.");
            } 

        }

        public Task<bool> DeleteBasketAsync(string id)
        {
            return _basketRepository.DeleteBasketAsync(id);
        }

        public async Task<BasketDto> GetBasketAsync(string key)
        {
            var basket = await _basketRepository.GetBasketAsync(key);
            if (basket is not null)
            {
                return _mapper.Map<BasketDto>(basket);
            }
            else
            {
                throw new BasketNotFoundException(key);
            }
        }
    }
}
