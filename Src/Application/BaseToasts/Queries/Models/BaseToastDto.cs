using Application.Accounts.Queries.Models;
using Application.Common.Mappings;
using Domain.Common;

namespace Application.BaseToasts.Queries.Models;

public class BaseToastDto : IMapFrom<BaseToast>
{
    public int Id { get; set; }
    public DateTime LastModified { get; set; }
    public DateTime Created { get; set; }
    public AccountBriefDto Author { get; set; } = null!;
    public string Type { get; set; } = string.Empty;
}