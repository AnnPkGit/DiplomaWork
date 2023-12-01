using Domain.Entities;
using Infrastructure.Persistence.Constants;
using Infrastructure.Persistence.RelationshipTables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable(TableNames.Roles);
        
        builder.HasKey(r => r.Id);

        builder.HasMany(r => r.Permissions)
            .WithMany()
            .UsingEntity<RolePermission>();

        builder.HasMany<User>()
            .WithMany()
            .UsingEntity<RoleUser>();

        builder.HasData(Role.GetValues());
    }
}