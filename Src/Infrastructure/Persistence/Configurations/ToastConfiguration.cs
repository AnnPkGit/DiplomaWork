using Domain.Entities;
using Infrastructure.Persistence.Constants;
using Infrastructure.Persistence.RelationshipTables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ToastConfiguration : IEntityTypeConfiguration<Toast>
{
    public void Configure(EntityTypeBuilder<Toast> builder)
    {
        builder.ToTable(TableNames.Toasts);
        builder.HasMany(t => t.MediaItems)
            .WithMany()
            .UsingEntity<ToastMediaItem>();
        
        builder.HasMany(t => t.Reactions)
            .WithMany()
            .UsingEntity<ToastReaction>();

        builder.HasMany(t => t.ReToasters)
            .WithMany()
            .UsingEntity<ReToast>();

        builder.HasOne(t => t.Quote)
            .WithMany()
            .IsRequired(false);
    }
}