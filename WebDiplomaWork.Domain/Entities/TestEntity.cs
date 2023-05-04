using System.ComponentModel.DataAnnotations.Schema;

namespace WebDiplomaWork.Domain.Entities;

[Table("test")]
public class TestEntity
{
    public String Id { get; set; }
    public String Name { get; set; }
}