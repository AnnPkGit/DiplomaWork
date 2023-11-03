using Domain.Entities.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ReToastNotificationConfiguration : IEntityTypeConfiguration<ReToastNotification>
{
    public void Configure(EntityTypeBuilder<ReToastNotification> builder)
    {
        builder.HasOne(rtn => rtn.ReToast)
            .WithMany()
            .HasForeignKey(rtn => rtn.ReToastId);
    }
}