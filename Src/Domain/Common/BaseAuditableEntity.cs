namespace Domain.Common;

public abstract class BaseAuditableEntity : BaseEntity, IDeactivated
{
    public DateTime? Deactivated { get; set; }
    
    public string? DeactivatedBy { get; set; }
    
    public DateTime Created { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? LastModified { get; set; }

    public string? LastModifiedBy { get; set; }
}