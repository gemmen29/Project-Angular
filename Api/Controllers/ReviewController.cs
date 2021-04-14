using BL.AppServices;
using BL.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        IHttpContextAccessor _httpContextAccessor;
        ReviewsAppService _reviewsAppService;
        public ReviewController(ReviewsAppService reviewsAppService,IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._reviewsAppService = reviewsAppService;
        }
        [HttpPost]
        public IActionResult addRating(int prodID, string description, int rating)
        {
            var userID = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            ReviewsViewModel reviewsViewModel = new ReviewsViewModel
            {

                Description = description,
                productID = prodID,
                rating = rating,
                userID = userID
            };
          var result=  _reviewsAppService.AddOrUpdateReview(reviewsViewModel);
            if (result)
                return Ok("ratnig added successfullly");
            return Content("Due to error rating not added");

        }
    }
}
