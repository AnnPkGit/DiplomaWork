using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class BaseToastConfiguration : IEntityTypeConfiguration<BaseToast>
{
    public void Configure(EntityTypeBuilder<BaseToast> builder)
    {
        builder.HasDiscriminator(bt => bt.Type);

        builder.Navigation(bt => bt.Author).AutoInclude();
    }
}