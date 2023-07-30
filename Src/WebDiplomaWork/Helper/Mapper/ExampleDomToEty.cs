using AutoMapper;
using Domain.Entity;
using Infrastructure.DbAccess.Entity;

namespace WebDiplomaWork.Helper.Mapper;

public class ExampleDomToEty : Profile
{
    public ExampleDomToEty()
    {
        CreateMap<TestExampleEntity, ExampleItem>();
    }
}