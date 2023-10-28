using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class BaseToastWithContextConfiguration : IEntityTypeConfiguration<BaseToastWithContent>
{
    public void Configure(EntityTypeBuilder<BaseToastWithContent> builder)
    {
        builder.HasMany(entity => entity.MediaItems).WithOne();
        
        builder.HasMany(entity => entity.Reactions)
            .WithOne(r => r.ToastWithContent)
            .HasForeignKey(r => r.ToastWithContentId);

        builder.HasMany(entity => entity.Replies)
            .WithOne(r => r.ReplyToToast)
            .HasForeignKey(r => r.ReplyToToastId);

        builder.HasMany(entity => entity.Quotes)
            .WithOne(q => q.QuotedToast)
            .HasForeignKey(q => q.QuotedToastId);
        
        builder.HasMany(entity => entity.ReToasts)
            .WithOne(rt => rt.ToastWithContent)
            .HasForeignKey(rt => rt.ToastWithContentId);
        
        builder.Navigation(entity => entity.MediaItems).AutoInclude();
    }
}