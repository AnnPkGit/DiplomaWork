using Application.BaseToasts.Queries.Models;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Toasts.Queries.Models;

public class ToastDto : BaseToastWithContentDto, IMapFrom<Toast>
{
    public ToastDto()
    {
    }

    public ToastDto(ICurrentUserService userService) : base(userService) {}
    public override void Mapping(Profile profile)
    {
        profile.CreateMap<Toast, ToastDto>();
        base.Mapping(profile);
    }
}