using System.Linq.Expressions;
using System.Reflection;
using Application.Common.Interfaces;
using Domain.Common;
using Domain.Entities;
using Domain.Entities.Notifications;
using Infrastructure.Persistence.Interceptors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly IMediator _mediator;
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor,
        IMediator mediator) : base(options)
    {
        _mediator = mediator;
        _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Reaction> Reactions => Set<Reaction>();
    public DbSet<BaseToast> BaseToasts => Set<BaseToast>();
    public DbSet<BaseToastWithContent> BaseToastsWithContent => Set<BaseToastWithContent>();
    public DbSet<Toast> Toasts => Set<Toast>();
    public DbSet<ReToast> ReToasts => Set<ReToast>();
    public DbSet<Reply> Replies => Set<Reply>();
    public DbSet<Quote> Quotes => Set<Quote>();
    public DbSet<Follow> Follows => Set<Follow>();

    public DbSet<BaseNotification> BaseNotifications => Set<BaseNotification>();
    public DbSet<ReactionNotification> ReactionNotifications => Set<ReactionNotification>();
    public DbSet<ReToastNotification> ReToastNotifications => Set<ReToastNotification>();
    public DbSet<QuoteNotification> QuoteNotifications => Set<QuoteNotification>();
    public DbSet<ReplyNotification> ReplyNotifications => Set<ReplyNotification>();
    public DbSet<ToastMediaItem> ToastMediaItems => Set<ToastMediaItem>();
    public DbSet<AvatarMediaItem> AvatarMediaItems => Set<AvatarMediaItem>();
    

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        Expression<Func<BaseAuditableEntity, bool>> filterExpr = be => be.Deactivated == null;
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            // check if current entity type is child of BaseAuditableEntity
            if (!entityType.ClrType.IsAssignableTo(typeof(BaseAuditableEntity)) || entityType.BaseType != null) continue;
            // modify expression to handle correct child type
            var parameter = Expression.Parameter(entityType.ClrType);
            var body = ReplacingExpressionVisitor.Replace(
                filterExpr.Parameters.First(), 
                parameter, 
                filterExpr.Body);
            var lambdaExpression = Expression.Lambda(body, parameter);

            // set filter
            entityType.SetQueryFilter(lambdaExpression); // <-- must come after all entity definitions
        }
        
        base.OnModelCreating(builder);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
    }
    
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _mediator.DispatchDomainEvents(this);

        return await base.SaveChangesAsync(cancellationToken);
    }
}