using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class BaseNotificationConfiguration : IEntityTypeConfiguration<BaseNotification>
{
    public void Configure(EntityTypeBuilder<BaseNotification> builder)
    {
        builder.HasDiscriminator(bn => bn.Type);
        builder.HasQueryFilter(bn => bn.ToAccount.Deactivated == null);
    }
}