using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BL.AppServices;
using BL.ViewModels;
using Api.HelpClasses;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
            var res = _accountAppService.GetAllAccounts();
            return Ok(res);
        }

        [HttpPost("/Register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewodel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            /*bool isExist = _accountAppService.checkUserNameExist(model.UserName);
            if (isExist)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });*/

            var result = await _accountAppService.Register(model);

            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }
        [HttpPost("/Login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            var user = await _accountAppService.Find(model.UserName, model.PasswordHash);
            if (user != null )
            {
                dynamic token = await _accountAppService.CreateToken(user);
               
                return Ok(token);
            }
            return Unauthorized();
        }
    }
}
