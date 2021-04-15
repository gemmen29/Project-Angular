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
    public class CartController : ControllerBase
    {
        ProductCartAppService _productCartAppService;
        ProductAppService _productAppService; 
        PaymentAppService _paymentAppService; 
        CartAppService _cartAppService;
        IHttpContextAccessor _httpContextAccessor;

        public CartController(ProductCartAppService productCartAppService,
            ProductAppService productAppService ,
            PaymentAppService paymentAppService ,
            CartAppService cartAppService ,
            IHttpContextAccessor httpContextAccessor)
        {
            this._productCartAppService = productCartAppService;
            this._productAppService = productAppService;
            this._paymentAppService = paymentAppService;
            this._cartAppService = cartAppService;
            this._httpContextAccessor = httpContextAccessor;
        }
        [HttpGet]
        public ActionResult Index()
        {

            //get all products in specfic cart
            //firs get cart id of logged user
            var userID = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var cartID = _cartAppService.GetAllCarts().Where(c => c.ApplicationUserIdentity_Id == userID)
                                                           .Select(c => c.ID).FirstOrDefault();
            var productIDs = _productCartAppService.GetAllProductCart().Where(pc => pc.cartId == cartID).Select(prc => prc.productId);
            List<ProductViewModel> productViewModels = new List<ProductViewModel>();
            foreach (var proID in productIDs)
            {
                var product = _productAppService.GetPoduct(proID);
                productViewModels.Add(product);
            }
            CartAndPaymentInfoViewModel cardDetailsViewModel = new CartAndPaymentInfoViewModel
            {
                paymentViewModels = _paymentAppService.GetPaymentsOfUser(userID),
                productViewModels = productViewModels

            };
            return Ok(cardDetailsViewModel);
        }

        [HttpPost]
        public void AddProductToCart(int id)
        {
            //get cart of current logged user
            var userID = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var cartID = _cartAppService.GetAllCarts().Where(c => c.ApplicationUserIdentity_Id == userID)
                                                           .Select(c => c.ID).FirstOrDefault();
            var productCartViewModel = new ProductCartViewModel() { cartId = cartID, productId = id };
            var isExistingProductCartViewModel = _productCartAppService.GetAllProductCart()
                                                 .FirstOrDefault(c => c.cartId == productCartViewModel.cartId && c.productId == productCartViewModel.productId);

            if (isExistingProductCartViewModel == null)
                _productCartAppService.SaveNewProductCart(productCartViewModel);
            else
            {
                //this product exist in the cart you can not add it again
            }
            //return RedirectToAction("Index");

        }

        [HttpDelete]
        public ActionResult DeleteFromCart(int productID)
        {
            var userID = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var cartID = _cartAppService.GetAllCarts().Where(c => c.ApplicationUserIdentity_Id == userID)
                                                           .Select(c => c.ID).FirstOrDefault();
            var productCartViewModel = new ProductCartViewModel() { cartId = cartID, productId = productID };
            var deletedProductCart = _productCartAppService.GetAllProductCart()
                                                 .FirstOrDefault(c => c.cartId == productCartViewModel.cartId && c.productId == productCartViewModel.productId);

            _productCartAppService.DeleteProductCart(deletedProductCart.ID);
            return RedirectToAction("Index");
        }
    }
}
