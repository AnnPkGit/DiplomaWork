using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entity;

namespace Infrastructure.DbAccess.Entity;

[Table("test")]
public class ExampleEntity : ExampleItem
{
    public override Guid Id { get; set; }
    public override string Login { get; set; }
    public override string Name { get; set; }
}