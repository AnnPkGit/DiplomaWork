using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasMany(a => a.AllToasts)
            .WithOne(rt => rt.Author)
            .HasForeignKey(rt => rt.AuthorId);

        builder.HasMany(a => a.Reactions)
            .WithOne(r => r.Author)
            .HasForeignKey(r => r.AuthorId);

        builder.HasMany(a => a.MediaItems)
            .WithOne(r => r.Author)
            .HasForeignKey(r => r.AuthorId);
    }
}