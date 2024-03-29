﻿using Application.BaseToasts.Queries.Models;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Toasts.Queries.Models;

public class ToastBriefDto : BaseToastWithContentBriefDto, IMapFrom<Toast>
{

    public override void Mapping(Profile profile)
    {
        profile.CreateMap<Toast, ToastBriefDto>();
        base.Mapping(profile);
    }
}