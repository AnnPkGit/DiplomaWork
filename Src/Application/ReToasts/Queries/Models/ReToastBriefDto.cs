using Application.BaseToasts.Queries.Models;
using Application.Common.Mappings;
using Domain.Entities;

namespace Application.ReToasts.Queries.Models;

public class ReToastBriefDto : BaseToastDto, IMapFrom<ReToast>
{
    public BaseToastWithContentDto ToastWithContent { get; set; } = null!;
}