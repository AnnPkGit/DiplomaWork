using Domain.Entities;
using Infrastructure.Persistence.RelationshipTables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ToastReactionConfiguration : IEntityTypeConfiguration<ToastReaction>
{
    public void Configure(EntityTypeBuilder<ToastReaction> builder)
    {
        builder.HasKey(tr => new { tr.ToastId, tr.AccountId, tr.ReactionId });
    }
}