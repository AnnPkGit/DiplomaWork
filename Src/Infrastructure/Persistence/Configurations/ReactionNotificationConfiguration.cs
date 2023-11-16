using Domain.Entities.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ReactionNotificationConfiguration : IEntityTypeConfiguration<ReactionNotification>
{
    public void Configure(EntityTypeBuilder<ReactionNotification> builder)
    {
        builder.HasOne(rn => rn.Reaction)
            .WithMany()
            .HasForeignKey(rn => rn.ReactionId);
    }
}