using Application.Accounts.Queries.Models;
using Application.Common.Mappings;
using Application.MediaItems.Queries.Models;
using Domain.Entities;

namespace Application.Toasts.Queries.Models;

public class QuoteBriefDto : IMapFrom<Toast>, IMapFrom<ToastSelectModel>
{
    public int Id { get; set; }
    public DateTime LastModified { get; set; }
    public DateTime Created { get; set; }
    public AccountBriefDto? Author { get; set; }
    public string Context { get; set; } = string.Empty;
    
    public ICollection<MediaItemBriefDto> MediaItems { get; set; } = null!;
}