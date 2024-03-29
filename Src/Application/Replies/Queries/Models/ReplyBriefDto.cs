﻿using Application.BaseToasts.Queries.Models;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Replies.Queries.Models;

public class ReplyBriefDto : BaseToastWithContentBriefDto, IMapFrom<Reply>
{
    public ReplyToToastDto ReplyToToast { get; set; } = null!;
    
    public override void Mapping(Profile profile)
    {
        profile.CreateMap<Reply, ReplyBriefDto>();
        base.Mapping(profile);
    }
}

public class ReplyToToastDto : BaseToastBriefDto, IMapFrom<BaseToastWithContentDto>
{
}