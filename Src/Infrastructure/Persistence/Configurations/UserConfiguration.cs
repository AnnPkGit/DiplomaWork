using Domain.Entities;
using Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(TableNames.Users)
            .HasOne(e => e.Account)
            .WithOne(e => e.Owner)
            .HasForeignKey<Account>(e => e.Id)
            .IsRequired();
        
        builder.HasMany(e => e.Roles)
            .WithMany()
            .UsingEntity<RoleUser>();
        
        builder.Navigation(e => e.Roles).AutoInclude();
        builder.Navigation(e => e.Account).AutoInclude();
    }
}