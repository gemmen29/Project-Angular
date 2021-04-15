﻿using Microsoft.AspNetCore.Http;
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
using DAL;
using BL.StaticClasses;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private AccountAppService _accountAppService;
        private CartAppService _cartAppService;
        private WishlistAppService _wishlistAppService;
        private RoleAppService _roleAppService;
        public AccountController(AccountAppService accountAppService,CartAppService cartAppService,WishlistAppService wishlistAppService, RoleAppService roleAppService)
        {
            this._accountAppService = accountAppService;
            this._cartAppService = cartAppService;
            this._wishlistAppService = wishlistAppService;
            this._roleAppService = roleAppService;
        }
        [HttpGet]
        public IActionResult getAll()
        {
            var res = _accountAppService.GetAllAccounts();
            return Ok(res);
        }
        [HttpPost("/RegisterAdmin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterViewodel model)
        {

            return await Register(model, UserRoles.Admin);

        }

        [HttpPost("/Register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterViewodel model)
        {

            return await Register(model, UserRoles.User);
           
        }
        private async Task<IActionResult> Register(RegisterViewodel model, string roleName)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool isExist = await _accountAppService.checkUserNameExist(model.UserName);
            if (isExist)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            var result = await _accountAppService.Register(model);

            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            ApplicationUserIdentity identityUser = await _accountAppService.Find(model.UserName, model.PasswordHash);
            //signinmanager.SignIn(identityUser, true, true);
            //create cart for new user
            CartViewModel cartViewModel = new CartViewModel() { ApplicationUserIdentity_Id = identityUser.Id };
            _cartAppService.SaveNewCart(cartViewModel);

            WishlistViewModel wishlistViewModel = new WishlistViewModel() { ApplicationUserIdentity_Id = identityUser.Id };
            _wishlistAppService.SaveNewWishlist(wishlistViewModel);

            //create roles
           await _roleAppService.CreateRoles();
            await _accountAppService.AssignToRole(identityUser.Id, UserRoles.User);
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
