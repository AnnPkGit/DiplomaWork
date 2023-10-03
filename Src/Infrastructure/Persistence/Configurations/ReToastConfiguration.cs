using Infrastructure.Persistence.Constants;
using Infrastructure.Persistence.RelationshipTables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ReToastConfiguration : IEntityTypeConfiguration<ReToast>
{
    public void Configure(EntityTypeBuilder<ReToast> builder)
    {
        builder.ToTable(TableNames.ReToasts);
        builder.HasKey(rt => new { rt.ToastId, rt.AccountId });
    }
}