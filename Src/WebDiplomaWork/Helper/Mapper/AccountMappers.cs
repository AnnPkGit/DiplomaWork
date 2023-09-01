using App.Common.Interfaces.Services;
using AutoMapper;
using Domain.Entity;
using WebDiplomaWork.Controller;
using WebDiplomaWork.DTO;

namespace WebDiplomaWork.Helper.Mapper;

public sealed class AccountDtoToEty : Profile
{
    public AccountDtoToEty()
    {
        CreateMap<CreateAccountDto, Account>();
    }
}

public sealed class FullyUpdAccountDtoToMdl : Profile
{
    public FullyUpdAccountDtoToMdl()
    {
        CreateMap<FullyUpdateAccountDto, UpdateAccountModel>();
    }
}

public sealed class UpdAccountDtoToMdl : Profile
{
    public UpdAccountDtoToMdl()
    {
        CreateMap<UpdateAccountDto, UpdateAccountModel>();
    }
}