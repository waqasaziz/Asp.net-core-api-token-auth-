using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Models;
using Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebAPI.Helpers;

namespace WebAPI.Controllers
{
    [Authorize, ApiController, ApiVersion("1.0")]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreatePaymentRequest model)
        {
            var response = await _paymentService.CreatePayment(model, HttpContext.GetMerchant());

            if (response == null)
                return BadRequest(new { message = "Invalid merchent" });

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _paymentService.GetAll(HttpContext.GetMerchant());

            if (response == null)
                return BadRequest(new { message = "Invalid merchent" });

            return Ok(response);
        }

    }
}
