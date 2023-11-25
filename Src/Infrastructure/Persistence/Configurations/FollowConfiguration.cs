using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class FollowConfiguration : IEntityTypeConfiguration<Follow>
{
    public void Configure(EntityTypeBuilder<Follow> builder)
    {
        builder.HasOne(f => f.FollowTo)
            .WithMany(a => a.Followers)
            .HasForeignKey(f => f.FollowToId);
        
        builder.HasOne(f => f.FollowFrom)
            .WithMany(a => a.Follows)
            .HasForeignKey(f => f.FollowFromId);
        
        builder.HasQueryFilter(follow =>
            follow.FollowTo.Deactivated == null ||
            follow.FollowFrom.Deactivated == null);
    }
}