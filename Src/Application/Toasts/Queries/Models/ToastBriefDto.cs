using Application.BaseToasts.Queries.Models;
using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Toasts.Queries.Models;

public class ToastBriefDto : BaseToastWithContentDto, IMapFrom<Toast>
{
}