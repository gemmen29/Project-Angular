﻿using AutoMapper;
using BL.Dtos;
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
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Product, ProductViewModel>()
                //.ForMember(vm => vm.ColorName, vm => vm.MapFrom(m => m.Color.Name))
                //.ForMember(vm => vm.CategoryName, vm => vm.MapFrom(m => m.Category.Name))
                .ReverseMap()
                .ForMember(m => m.Color, m => m.Ignore())
                .ForMember(m => m.Category, m => m.Ignore());

           CreateMap<Order, OrderViewModel>().ReverseMap();
           CreateMap<OrderProduct, OrderProductViewModel>().ForMember(vm => vm.productName, m => m.MapFrom(u => u.Product.Name)).ReverseMap();

           CreateMap<IdentityRole, RoleViewModel>().ReverseMap();
           CreateMap<IdentityRole, UserRolesViewModel>().ReverseMap();
           CreateMap<Reviews, ReviewsViewModel>().ReverseMap();

           CreateMap<ProductCart, ProductCartViewModel>().ReverseMap();
           CreateMap<ProductWishList, ProductWishListViewModel>().ReverseMap();

           CreateMap<Category, CategoryViewModel>().ReverseMap();
           CreateMap<Payment, PaymentViewModel>().ReverseMap();


           CreateMap<Cart, CartViewModel>().ReverseMap();
           CreateMap<Wishlist, WishlistViewModel>().ReverseMap();

           CreateMap<ApplicationUserIdentity, LoginViewModel>().ReverseMap();
           CreateMap<ApplicationUserIdentity, RegisterViewodel>().ReverseMap();

           CreateMap<Color, ColorDTO>().ReverseMap();
        }
    }
}
