using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BL.AppServices;
using BL.Dtos;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private PaymentAppService _paymentAppService;
        public PaymentController(PaymentAppService paymentAppService)
        {
            _paymentAppService = paymentAppService;
        }
        [HttpGet]
        public IActionResult GetAllPayment()
        {
            var payments = _paymentAppService.GetAllPayments();
            return Ok(payments);
        }


        [HttpPost]
        public IActionResult Create(PaymentViewModel paymentViewModel)
        {
            //paymentViewModel.ApplicationUserIdentity_Id = User.Identity.GetUserId();
            var payments = _paymentAppService.GetAllPayments();
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);

            _paymentAppService.SaveNewPayment(paymentViewModel);

            return Ok();
        }


    }
}
