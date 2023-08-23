using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.EntityConf;

public class ExampleItemConfiguration : IEntityTypeConfiguration<ExampleItem>
{
    public void Configure(EntityTypeBuilder<ExampleItem> builder)
    {
        builder.ToTable("test");
    }
}