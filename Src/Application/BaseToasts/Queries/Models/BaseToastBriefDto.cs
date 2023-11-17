using Application.Accounts.Queries.Models;
using Application.Common.Mappings;
using Domain.Common;

namespace Application.BaseToasts.Queries.Models;

public class BaseToastBriefDto : IMapFrom<BaseToast>
{
    public int Id { get; set; }
    public AccountBriefDto Author { get; set; } = null!;
    public string Type { get; set; } = string.Empty;
}