using AutoMapper;
using Domain.Entity;
using WebDiplomaWork.DTO;

namespace WebDiplomaWork.Helper.Mapper
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<UserDto, User>();
        }
    }
}
