using App.Services;
using AutoMapper;
using WebDiplomaWork.DTO;

namespace WebDiplomaWork.Helper.Mapper;

public sealed class LoginDtoToReq : Profile
{
    public LoginDtoToReq()
    {
        CreateMap<LoginRequestDto, LoginRequest>().ReverseMap();
    }
}