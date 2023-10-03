using Domain.Entities;
using Infrastructure.Persistence.Constants;
using Infrastructure.Persistence.RelationshipTables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable(TableNames.Accounts);
        builder.HasMany(a => a.Toasts)
            .WithOne(t => t.Author)
            .HasForeignKey(t => t.AuthorId);
        
        builder.HasMany(a => a.ReToasts)
            .WithMany()
            .UsingEntity<ReToast>();
        
        builder.HasMany(a => a.MyReactions)
            .WithMany()
            .UsingEntity<ToastReaction>();
    }
}