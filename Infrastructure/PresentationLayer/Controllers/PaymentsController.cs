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

        //stripe listen --forward-to http://localhost:5139/api/Payments/webhook
        [HttpPost("webhook")]
        public async Task<IActionResult> Index()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            
            //Logic
            await _serviceManager.PaymentService.UpdatePaymentStatus(json,
                Request.Headers["Stripe-Signature"]);
            return new EmptyResult();
        }

    }
}
