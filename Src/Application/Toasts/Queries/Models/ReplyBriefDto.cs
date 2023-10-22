using Application.Accounts.Queries.Models;
using Application.Common.Mappings;
using Application.MediaItems.Queries.Models;
using Domain.Entities;

namespace Application.Toasts.Queries.Models;

public class ReplyBriefDto : IMapFrom<Toast>, IMapFrom<ToastSelectModel>
{
    public int Id { get; set; }
    public DateTime LastModified { get; set; }
    public DateTime Created { get; set; }
    public AccountBriefDto? Author { get; set; }
    public string Context { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    
    public bool ImReacted { get; set; }
    
    public int ReactionsCount { get; set; }
    public int RepliesCount { get; set; }
    public int ReToastsCount { get; set; }
    public int QuotesCount { get; set; }
    
    public ICollection<MediaItemBriefDto> MediaItems { get; set; } = null!;
}