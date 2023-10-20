using Application.Accounts.Queries.Models;
using Application.Common.Mappings;
using Application.MediaItems.Queries.Models;
using Domain.Entities;

namespace Application.Toasts.Queries.Models;

public class ToastBriefDto : IMapFrom<ToastSelectModel>, IMapFrom<Toast>
{
    public int Id { get; set; }
    public DateTime LastModified { get; set; }
    public DateTime Created { get; set; }
    public AccountBriefDto? Author { get; set; }
    public string Context { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    
    public ReplyBriefDto? Reply { get; set; }
    
    public QuoteBriefDto? Quote { get; set; }
    
    public ToastBriefDto? ReToast { get; set; }
    
    public bool ImReacted { get; set; }
    public int ReactionsCount { get; set; }
    public int RepliesCount { get; set; }
    public int ReToastsCount { get; set; }
    public int QuotesCount { get; set; }
    
    public IEnumerable<MediaItemBriefDto> MediaItems { get; set; } = null!;
    public IEnumerable<ReplyBriefDto> Thread { get; set; } = null!;
}