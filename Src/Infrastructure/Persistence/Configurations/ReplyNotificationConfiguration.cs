using Domain.Entities.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ReplyNotificationConfiguration : IEntityTypeConfiguration<ReplyNotification>
{
    public void Configure(EntityTypeBuilder<ReplyNotification> builder)
    {
        builder.HasOne(rn => rn.Reply)
            .WithMany()
            .HasForeignKey(rn => rn.ReplyId);
    }
}