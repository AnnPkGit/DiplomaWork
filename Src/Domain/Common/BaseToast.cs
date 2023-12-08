using System.Reflection;

namespace Domain.Common;

public class BaseToast : BaseAuditableEntity
{
    public BaseToast(){}
    
    protected BaseToast(int authorId, MemberInfo childType)
    {
        AuthorId = authorId;
        Type = childType.Name;
    }

    public int? AuthorId { get; set; }
    public Account? Author { get; set; }
    public string Type { get; set; } = string.Empty;
}