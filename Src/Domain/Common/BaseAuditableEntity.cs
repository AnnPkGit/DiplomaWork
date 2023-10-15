namespace Domain.Common;

public abstract class BaseAuditableEntity : BaseEntity
{
    public override int Id { get; init; }
    public virtual DateTime? Deactivated { get; set; }
    
    public int? DeactivatedById { get; set; }
    public User? DeactivatedBy { get; set; }
    
    public DateTime Created { get; set; }

    public DateTime? LastModified { get; set; }
}