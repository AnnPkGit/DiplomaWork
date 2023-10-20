using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasMany(a => a.Toasts)
            .WithOne(t => t.Author)
            .HasForeignKey(t => t.AuthorId);

        builder.HasMany(a => a.Reactions)
            .WithOne(r => r.Author)
            .HasForeignKey(r => r.AuthorId);

        builder.HasMany(a => a.MediaItems)
            .WithOne(r => r.Author)
            .HasForeignKey(r => r.AuthorId);
    }
}