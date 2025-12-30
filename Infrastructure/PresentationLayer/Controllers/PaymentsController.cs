using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstractionLayer;
using Shared.DataTransferObjects.BasketDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer.Controllers
{
    public class PaymentsController(IServiceManager _serviceManager) : ApiBaseController
    {
        //Create Or Update Payment Intent Id
        [Authorize]
        [HttpPost("{BasketId}")]
        public async Task<ActionResult<BasketDto>> CreateOrUpdatePaymentIntent(string BasketId)
        {
            var basket = await _serviceManager.PaymentService.CreateOrUpdatePaymentIntent(BasketId);
            return Ok(basket);
        }

    }
}
