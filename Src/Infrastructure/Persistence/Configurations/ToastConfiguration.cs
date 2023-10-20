using Domain.Entities;
using Infrastructure.Persistence.RelationshipTables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ToastConfiguration : IEntityTypeConfiguration<Toast>
{
    public void Configure(EntityTypeBuilder<Toast> builder)
    {
        builder.HasMany(t => t.MediaItems)
            .WithMany(mi => mi.Toasts)
            .UsingEntity<ToastMediaItem>();

        builder.HasMany(t => t.Reactions)
            .WithOne(t => t.Toast)
            .HasForeignKey(t => t.ToastId);
        
        builder.HasMany(t => t.ReToasts)
            .WithOne(t => t.ReToast)
            .HasForeignKey(t => t.ReToastId);

        builder.HasOne(t => t.Quote)
            .WithMany(t => t.Quotes)
            .HasForeignKey(t => t.QuoteId);

        builder.HasOne(t => t.Reply)
            .WithMany(t => t.Replies)
            .HasForeignKey(t => t.ReplyId);

        builder.Navigation(t => t.Author).AutoInclude();
        builder.Navigation(t => t.MediaItems).AutoInclude();
    }
}