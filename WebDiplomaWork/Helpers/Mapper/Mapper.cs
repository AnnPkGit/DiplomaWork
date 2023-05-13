using AutoMapper;
using WebDiplomaWork.DB.DTOs;
using WebDiplomaWork.Domain.Entities;

namespace WebDiplomaWork.Helpers.Mapper
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<UserDto, UserEntity>();
        }
    }
}
