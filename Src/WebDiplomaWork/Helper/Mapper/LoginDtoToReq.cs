using App.Users.Login;
using AutoMapper;
using WebDiplomaWork.DTO;

namespace WebDiplomaWork.Helper.Mapper;

public sealed class LoginDtoToReq : Profile
{
    public LoginDtoToReq()
    {
        CreateMap<LoginRquestDto, LoginRequest>().ReverseMap();
    }
}