using Domain.Common;
using Domain.Entities;
using Domain.Entities.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasMany<BaseToast>()
            .WithOne(rt => rt.Author)
            .HasForeignKey(rt => rt.AuthorId);

        builder.HasMany<Reaction>()
            .WithOne(r => r.Author)
            .HasForeignKey(r => r.AuthorId);

        builder.HasMany<BaseMediaItem>()
            .WithOne(r => r.Author)
            .HasForeignKey(r => r.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.Avatar)
            .WithMany()
            .HasForeignKey(a => a.AvatarId);
        
        builder.HasOne(a => a.Banner)
            .WithMany()
            .HasForeignKey(a => a.BannerId);

        builder.HasMany<BaseNotification>()
            .WithOne(bn => bn.ToAccount)
            .HasForeignKey(bn => bn.ToAccountId);

        builder.HasMany<FollowerNotification>()
            .WithOne(fn => fn.Follower)
            .HasForeignKey(fn => fn.FollowerId);
        
        builder.Navigation(a => a.Avatar).AutoInclude();
    }
}