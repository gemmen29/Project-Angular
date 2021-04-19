using BL.AppServices;
using BL.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        OrderAppService _orderAppService;
        CartAppService _cartAppService;
        ProductCartAppService _productCartAppService ;
        ProductAppService _productAppService;
        OrderProductAppService _orderProductAppService;
        IHttpContextAccessor _httpContextAccessor;
        public OrderController(OrderAppService orderAppService,
            CartAppService cartAppService,
            ProductCartAppService productCartAppService,
            ProductAppService productAppService,
            OrderProductAppService orderProductAppService,
            IHttpContextAccessor httpContextAccessor)
        {
            this._orderAppService = orderAppService;
            this._cartAppService = cartAppService;
            this._productCartAppService = productCartAppService;
            this._productAppService = productAppService;
            this._orderProductAppService = orderProductAppService;
            this._httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return Ok(_orderAppService.GetAllOrder());
        }



        [HttpPost]
        public IActionResult makeOrder(int[] quantities, double totalOrderPrice)
        {

            //get cart id of current logged user
            var userID = "19";
            //var userID = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var cartID = _cartAppService.GetAllCarts().Where(c => c.ApplicationUserIdentity_Id == userID)
                                                           .Select(c => c.ID).FirstOrDefault();
            //get product ids from this card
            var prodIds = _productCartAppService.GetAllProductCart().Where(pc => pc.cartId == cartID)
                                                                 .Select(pc => pc.productId).ToList();

            OrderViewModel orderViewModel = new OrderViewModel
            {
                date = DateTime.Now.ToString(),

                totalPrice = totalOrderPrice,
                ApplicationUserIdentity_Id = userID

            };
            _orderAppService.SaveNewOrder(orderViewModel);
            var lastOrder = _orderAppService.GetAllOrder().Select(o => o.Id).LastOrDefault();

            //get know details of each product
            for (int i = 0; i < prodIds.Count; i++)
            {

                var productViewModel = _productAppService.GetPoduct(prodIds[i]);
                double totOrder = productViewModel.Price * quantities[i];
                double AfterDiscount = totOrder - totOrder * (productViewModel.Discount / 100);
                OrderProductViewModel orderProductViewModel = new OrderProductViewModel
                {
                    orderID = lastOrder,
                    ProductID = productViewModel.ID,
                    productDiscount = productViewModel.Discount,
                    productQuantity = quantities[i],
                    productTotal = totOrder,
                    ProductNetPrice = AfterDiscount
                };
                _orderProductAppService.SaveNewOrderProduct(orderProductViewModel);
                //decrease amount of product
                _productAppService.DecreaseQuantity(productViewModel.ID, quantities[i]);


                var productCartID = _productCartAppService.GetAllProductCart()
                                                       .Where(prc => prc.cartId == cartID && prc.productId == productViewModel.ID)
                                                       .Select(prc => prc.ID).FirstOrDefault();
                _productCartAppService.DeleteProductCart(productCartID);

            }

            //notify admin by the product that had been bought
            var productQuantities = new List<ProductQuantitiesCheckoutViewModel>();
            for (int i = 0; i < quantities.Length; i++)
            {
                productQuantities.Add(
                new ProductQuantitiesCheckoutViewModel
                {
                    ProductID = prodIds[i],
                    Quantity = quantities[i]
                });
            }
            return RedirectToAction("index", "home");
        }

        //[HttpGet]
        //Route[("api/[controller]/{id}")]
        [HttpGet("{id}")]
        public IActionResult Details(int id)
        {
            return Ok(_orderProductAppService.GetAllOrderProduct().Where(op => op.orderID == id).ToList());
        }
    }
}
