using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entity;

namespace Infrastructure.DbAccess.Entity;

[Table("test")]
public class TestExampleEntity
{
    public Guid Id { get; set; }
    public string Login { get; set; }
    public string Name { get; set; }
}