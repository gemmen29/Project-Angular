using AutoMapper;
using BL.ViewModels;
using DAL;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Configurations
{
    public static class MapperConfig
    {
        public static IMapper Mapper { get; set; }
        static MapperConfig()
        {
            var config = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<Order, OrderViewModel>().ReverseMap();
                    cfg.CreateMap<OrderProduct, OrderProductViewModel>().ForMember(vm => vm.productName, m => m.MapFrom(u=>u.Product.Name)).ReverseMap();
                                              
                    cfg.CreateMap<IdentityRole, RoleViewModel>().ReverseMap();
                    cfg.CreateMap<IdentityRole, UserRolesViewModel>().ReverseMap();
                    cfg.CreateMap<Reviews, ReviewsViewModel>().ReverseMap();

                    cfg.CreateMap<ProductCart, ProductCartViewModel>().ReverseMap();
                    cfg.CreateMap<ProductWishList, ProductWishListViewModel>().ReverseMap();

                    cfg.CreateMap<Category, CategoryViewModel>().ReverseMap();
                    cfg.CreateMap<Payment, PaymentViewModel>().ReverseMap();
                    cfg.CreateMap<ProductDto, Product>().ReverseMap();

                    cfg.CreateMap<Cart, CartViewModel>().ReverseMap();
                    cfg.CreateMap<Wishlist, WishlistViewModel>().ReverseMap();

                    cfg.CreateMap<Product, ProductViewModel>().ReverseMap();
                    cfg.CreateMap<ProductViewModel, ProductViewModel>().ReverseMap();

                    cfg.CreateMap<ApplicationUserIdentity, LoginViewModel>().ReverseMap();
                    cfg.CreateMap<ApplicationUserIdentity, RegisterViewodel>().ReverseMap();
                    //cfg.CreateMap<IdentityResult, ResultStatue>().ReverseMap();

                });
            Mapper = config.CreateMapper();
        }
    }
}
