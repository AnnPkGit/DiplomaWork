using Domain.Entities;
using Infrastructure.Persistence.Constants;
using Infrastructure.Persistence.RelationshipTables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(TableNames.Users)
            .HasOne(u => u.Account)
            .WithOne(a => a.Owner)
            .HasForeignKey<Account>(a => a.Id)
            .IsRequired();
        
        builder.HasMany(u => u.Roles)
            .WithMany()
            .UsingEntity<RoleUser>();

        builder.HasMany<User>()
            .WithOne(t => t.DeactivatedBy)
            .HasForeignKey(t => t.DeactivatedById);
        
        builder.Navigation(u => u.Roles).AutoInclude();
        builder.Navigation(u => u.Account).AutoInclude();
    }
}