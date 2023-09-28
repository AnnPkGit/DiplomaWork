namespace Domain.Common;

public class BasicLegalEntity : BaseEntity, IDeactivated
{
    public virtual DateTime? Deactivated { get; set; }
    
    public DateTime Created { get; set; }
    
    public DateTime? LastModified { get; set; }
}