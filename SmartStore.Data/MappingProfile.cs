using AutoMapper;
using SmartStore.Data.Entities;
using SmartStore.Data.Models;

namespace SmartStore.Data
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductModel>().ReverseMap();
            CreateMap<StockMovement, ProductModel>()
                .ForMember(p => p.Name, opt => opt.MapFrom(s => s.Product.Name))
                .ForMember(p => p.Description, opt => opt.MapFrom(s => s.Product.Description))
                .ForMember(p => p.SellingPrice, opt => opt.MapFrom(s => s.Product.SellingPrice))
                .ForMember(p => p.StockBalance, opt => opt.MapFrom(s => s.Balance))
                .ForMember(p => p.LastStockMovementDate, opt => opt.MapFrom(s => s.Date));
            CreateMap<UserEntity, UserDetails>();
        }
    }
}
