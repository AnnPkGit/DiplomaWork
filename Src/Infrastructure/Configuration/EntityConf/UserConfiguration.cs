using Domain.Entities;
using Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.EntityConf;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(TableNames.Users)
            .HasMany(e => e.Accounts)
            .WithOne(e => e.Owner)
            .HasForeignKey(e => e.OwnerId)
            .IsRequired();
        
        builder.HasMany(e => e.Roles)
            .WithMany()
            .UsingEntity<RoleUser>();
        
        builder.Navigation(e => e.Roles).AutoInclude();
    }
}