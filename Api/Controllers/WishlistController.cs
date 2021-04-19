using BL.AppServices;
using BL.Dtos;
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
    public class WishlistController : ControllerBase
    {
        ProductWishListAppService _productWishListAppService;
        ProductAppService _productAppService;
        WishlistAppService _wishlistAppService;
        IHttpContextAccessor _httpContextAccessor;
        public WishlistController(ProductWishListAppService productWishListAppService,
                                  ProductAppService productAppService,
                                  WishlistAppService wishlistAppService,
                                  IHttpContextAccessor httpContextAccessor)

        {
            this._productAppService = productAppService;
            this._wishlistAppService = wishlistAppService;
            this._productWishListAppService = productWishListAppService;
            this._httpContextAccessor = httpContextAccessor;
        }
        [HttpGet]
        public IActionResult getUserWishList()
        {
            //get all products in specfic wishlist
            //firs get cart id of logged user
           var userID = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var wishListID = _wishlistAppService.GetAllWishlists().Where(w => w.ApplicationUserIdentity_Id == userID)
                                                           .Select(w => w.ID).FirstOrDefault();
            var productIDs = _productWishListAppService.GetAllProductWishList().Where(w => w.wishlistId == wishListID).Select(wpr => wpr.productId);
            List<ProductViewModel> productViewModels = new List<ProductViewModel>();
            foreach (var proID in productIDs)
            {
                var product = _productAppService.GetPoduct(proID);
                productViewModels.Add(product);
            }
            return Ok(productViewModels);
        }
        //[HttpPut]
        [HttpPut("{id}")]
        //make it as httpPut because we will update on user wishlist
        public IActionResult AddProductToWishList(int id)
        {
            //get wishlist of current logged user
            //var userID = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userID = "d323a82b-0292-4844-aa74-009dbcff12d1";
            var wishListID = _wishlistAppService.GetAllWishlists().Where(w => w.ApplicationUserIdentity_Id == userID)
                                                           .Select(w => w.ID).FirstOrDefault();
            var productWishListViewModel = new ProductWishListViewModel() { wishlistId = wishListID, productId = id };
            var isExistingProductWishListViewModel = _productWishListAppService.GetAllProductWishList()
                                                .FirstOrDefault(w => w.wishlistId == productWishListViewModel.wishlistId && w.productId == productWishListViewModel.productId);

            if (isExistingProductWishListViewModel == null)
            {
                _productWishListAppService.SaveNewProductWishlist(productWishListViewModel);
                return Ok();
            }
                //if product already exist
                return BadRequest("this product already exist in wishList");
                //this product exist in the wishlist you can not add it again
        }
        //[HttpDelete]
        [HttpDelete("{producID}")]
        public IActionResult DeleteFromWishList(int producID)
        {
            var userID = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var wishListID = _wishlistAppService.GetAllWishlists().Where(w => w.ApplicationUserIdentity_Id == userID)
                                                           .Select(w => w.ID).FirstOrDefault();
            var productWishlistViewModel = new ProductWishListViewModel() { wishlistId = wishListID, productId = producID };
            var deletedProductWishList = _productWishListAppService.GetAllProductWishList()
                                                 .FirstOrDefault(w => w.wishlistId == productWishlistViewModel.wishlistId && w.productId == productWishlistViewModel.productId);

            var isDeleted = _productWishListAppService.DeleteProductWishList(deletedProductWishList.ID);
            if (isDeleted)
                return Content("Deleted succfully");
            return Content("Error Occur In Deletion");


        }
    }
}
