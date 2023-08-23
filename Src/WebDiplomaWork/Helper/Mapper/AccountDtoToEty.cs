using AutoMapper;
using Domain.Entity;
using WebDiplomaWork.DTO;

namespace WebDiplomaWork.Helper.Mapper;

public sealed class AccountDtoToEty : Profile
{
    public AccountDtoToEty()
    {
        CreateMap<CreateAccountDto, Account>();
    }
}