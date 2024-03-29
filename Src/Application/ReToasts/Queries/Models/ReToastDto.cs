﻿using Application.BaseToasts.Queries.Models;
using Application.Common.Mappings;
using Domain.Entities;

namespace Application.ReToasts.Queries.Models;

public class ReToastDto : BaseToastDto, IMapFrom<ReToast>
{
    public BaseToastWithContentBriefDto ToastWithContent { get; set; } = null!;
}