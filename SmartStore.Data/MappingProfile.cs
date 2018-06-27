using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using SmartStore.Data.Entities;
using SmartStore.Data.Models;

namespace SmartStore.Data
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Product to ProductModel, then reverse
            CreateMap<Product, ProductModel>()
                .ForMember(pm => pm.Tags, opt => opt.MapFrom(p => p.Tags.Select(t => t.Name).ToArray()))
                .ReverseMap()
                    .ForMember(p => p.Tags, opt => opt.ResolveUsing<TagsCollectionResolver>());
            //StockMovement to ProductModel
            CreateMap<StockMovement, ProductModel>()
                .ForMember(p => p.Id, opt => opt.MapFrom(s => s.Product.Id))
                .ForMember(p => p.Name, opt => opt.MapFrom(s => s.Product.Name))
                .ForMember(p => p.Description, opt => opt.MapFrom(s => s.Product.Description))
                .ForMember(p => p.SellingPrice, opt => opt.MapFrom(s => s.Product.SellingPrice))
                .ForMember(p => p.StockBalance, opt => opt.MapFrom(s => s.Balance))
                .ForMember(p => p.LastStockMovementDate, opt => opt.MapFrom(s => s.Date));
            //StockMovementType to SelectListItem (to be used in DropdownLists = <select>)
            CreateMap<StockMovementType, SelectListItem>()
                .ForMember(i => i.Value, opt => opt.MapFrom(t => t.Id))
                .ForMember(i => i.Text, opt => opt.MapFrom(t => t.Name));
            //UserEntity to UserDetails
            CreateMap<UserEntity, UserDetails>();
            //PurchaseOrder and PurchaseOrderItem to OrderModel and OrderItemModel
            CreateMap<PurchaseOrderItem, OrderItemModel>()
                .ForMember(o => o.Status, opt => opt.MapFrom(p => p.Status.Name));
            CreateMap<PurchaseOrder, OrderModel>()
                .ForMember(o => o.Status, opt => opt.MapFrom(p => p.Status.Name));
        }
    }
}
