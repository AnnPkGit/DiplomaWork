using System.Reflection;

namespace Domain.Common;

public class BaseNotification
{
    public BaseNotification()
    {
    }

    public BaseNotification(int toAccountId, MemberInfo childType, DateTime created)
    {
        ToAccountId = toAccountId;
        Created = created;
        Type = childType.Name;
    }

    public int Id { get; set; }
    
    public int ToAccountId { get; set; }

    public Account ToAccount { get; set; } = null!;

    public string Type { get; set; } = string.Empty;
    
    public DateTime Created { get; set; }
    public DateTime? Viewed { get; set; }
}