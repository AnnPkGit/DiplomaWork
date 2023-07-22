using Infrastructure.DbAccess.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DbAccess.EfDbContext;

public class ExampleContext : DbContext
{
    public DbSet<ExampleEntity> Example { get; private set; }
    
    public ExampleContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySQL("Server=localhost;Port=3306;Database=test;Uid= ;Pwd= ;");
    }
}