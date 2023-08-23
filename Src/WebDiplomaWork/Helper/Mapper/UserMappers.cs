using AutoMapper;
using Domain.Entity;
using WebDiplomaWork.DTO;

namespace WebDiplomaWork.Helper.Mapper;

public class CreateUserDtoToEty : Profile
{
    public CreateUserDtoToEty()
    {
        CreateMap<UserRegistrationDto, User>();
    }
}