using Domain.Entities;
using Infrastructure.Persistence.RelationshipTables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ToastMediaConfiguration : IEntityTypeConfiguration<ToastMediaItem>
{
    public void Configure(EntityTypeBuilder<ToastMediaItem> builder)
    {
        builder.HasKey(tm => new { tm.ToastId, tm.MediaItemId });
    }
}