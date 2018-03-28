using AutoMapper;
using SmartStore.Data.Entities;
using SmartStore.Domain.Models;

namespace SmartStore.Service
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductModel>();
        }
    }
}
