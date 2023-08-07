using AutoMapper;
using Domain.Entity;
using Infrastructure.DbAccess.Entity;
using WebDiplomaWork.DTO;

namespace WebDiplomaWork.Helper.Mapper
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<UserDto, User>();
           CreateMap<UserRegistrationDto, User>();
           CreateMap<User, UserEntity>();
           CreateMap<UserEntity, User>();
        }
    }
}
