namespace Domain.Common;

public abstract class BaseAuditableEntity : BaseEntity
{
    // TODO: public string? DeactivatedBy { get; set; }
    
    public DateTime Created { get; set; }

    // TODO: public string? CreatedBy { get; set; }

    public DateTime? LastModified { get; set; }

    // TODO: public string? LastModifiedBy { get; set; }
}