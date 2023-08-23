using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.EntityConf;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users")
            .HasMany(e => e.Accounts)
            .WithOne(e => e.Owner)
            .HasForeignKey("UserId")
            .IsRequired();
    }
}