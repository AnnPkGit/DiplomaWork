using Domain.Entities;
using Infrastructure.Persistence.RelationshipTables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class MuteNotificationOptionConfiguration : IEntityTypeConfiguration<MuteNotificationOption>
{
    public void Configure(EntityTypeBuilder<MuteNotificationOption> builder)
    {
        builder.HasKey(option => option.Id);

        builder.HasMany<User>()
            .WithMany(user => user.MuteNotificationOptions)
            .UsingEntity<MuteNotificationOptionUser>();
        
        builder.HasData(MuteNotificationOption.GetValues());
    }
}