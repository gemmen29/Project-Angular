using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BL.AppServices;
using BL.ViewModels;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private AccountAppService _accountAppService;
        public AccountController(AccountAppService accountAppService)
        {
            this._accountAppService = accountAppService;
        }
        [HttpGet]
        public IActionResult getAll()
        {
          var res=  _accountAppService.GetAllAccounts();
            return Ok(res);
        }
        [HttpPost]
        public async Task <IActionResult> Register(RegisterViewodel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
           await _accountAppService.Register(registerViewModel);
            return Ok();
        }
    }
}
