using Shared.DataTransferObjects.BasketDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstractionLayer
{
    public interface IPaymentService
    {
        Task<BasketDto> CreateOrUpdatePaymentIntent(string basketId);
        Task UpdatePaymentStatus(string jsonRequest, string stripeHeader);
    }
}
