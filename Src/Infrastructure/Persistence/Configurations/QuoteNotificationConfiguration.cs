using Domain.Entities.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class QuoteNotificationConfiguration : IEntityTypeConfiguration<QuoteNotification>
{
    public void Configure(EntityTypeBuilder<QuoteNotification> builder)
    {
        builder.HasOne(qn => qn.Quote)
            .WithMany()
            .HasForeignKey(qn => qn.QuoteId);
    }
}