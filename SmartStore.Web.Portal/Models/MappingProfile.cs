﻿using AutoMapper;
using SmartStore.Data.Entities;

namespace SmartStore.Web.Portal.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<NewUserModel, UserEntity>();
            CreateMap<BillingInformationModel, UserEntity>()
                .ReverseMap()
                    .ForMember(b => b.UserId, opt => opt.MapFrom(u => u.Id));
            CreateMap<CartItemModel, CartItem>()
                .ForMember(c => c.Product, opt => opt.MapFrom(m => new Product() { Id = m.ProductId }))
                .ReverseMap()
                    .ForMember(c => c.ProductId, opt => opt.MapFrom(e => e.Product.Id));
            CreateMap<CartModel, ShoppingCart>()
                .ReverseMap();
        }
    }
}
