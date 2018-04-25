using AutoMapper;
using SmartStore.Data.Entities;

namespace SmartStore.Web.Portal.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<NewUserModel, UserEntity>();
        }
    }
}
