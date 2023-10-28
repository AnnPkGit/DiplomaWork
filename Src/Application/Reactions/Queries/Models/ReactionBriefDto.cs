using Application.Accounts.Queries.Models;
using Application.BaseToasts.Queries.Models;
using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Reactions.Queries.Models;

public class ReactionBriefDto : IMapFrom<Reaction>
{
    public AccountBriefDto Author { get; set; } = null!;
    public BaseToastWithContentDto ToastWithContent { get; set; } = null!;
}