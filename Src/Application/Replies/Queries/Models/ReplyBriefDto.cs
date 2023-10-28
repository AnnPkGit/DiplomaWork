using Application.BaseToasts.Queries.Models;
using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Replies.Queries.Models;

public class ReplyBriefDto : BaseToastWithContentDto, IMapFrom<Reply>
{
    public BaseToastDto ReplyToToast { get; set; } = null!;
}