namespace Domain.Common;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime? DeactivationDate { get; set; }
}