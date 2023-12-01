using Domain.Entities;
using Infrastructure.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable(TableNames.Permissions);

        builder.HasKey(p => p.Id);

        var permissions = Enum.GetValues<Domain.Enums.Permission>()
            .Select(p => new Permission((int)p, p.ToString()));
        
        builder.HasData(permissions);
    }
}