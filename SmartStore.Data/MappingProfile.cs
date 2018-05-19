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
            CreateMap<UserEntity, UserDetails>();
        }
    }
}
