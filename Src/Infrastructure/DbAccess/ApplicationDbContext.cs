using System.Linq.Expressions;
using System.Reflection;
using Application.Common.Interfaces;
using Domain.Common;
using Domain.Entities;
using Infrastructure.Configuration.Provider;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Infrastructure.DbAccess;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly IMediator _mediator;
    private readonly IDbAccessProvider _dbAccessProvider;
    public ApplicationDbContext(
        IDbAccessProvider dbAccessProvider,
        IMediator mediator)
    {
        _dbAccessProvider = dbAccessProvider;
        _mediator = mediator;
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Account> Accounts => Set<Account>();
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        Expression<Func<BaseEntity, bool>> filterExpr = be => be.Deactivated == null;
        foreach (var mutableEntityType in builder.Model.GetEntityTypes())
        {
            // check if current entity type is child of BaseModel
            if (!mutableEntityType.ClrType.IsAssignableTo(typeof(BaseEntity))) continue;
            // modify expression to handle correct child type
            var parameter = Expression.Parameter(mutableEntityType.ClrType);
            var body = ReplacingExpressionVisitor.Replace(
                filterExpr.Parameters.First(), 
                parameter, 
                filterExpr.Body);
            var lambdaExpression = Expression.Lambda(body, parameter);

            // set filter
            mutableEntityType.SetQueryFilter(lambdaExpression); // <-- must come after all entity definitions
        }
        
        base.OnModelCreating(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = _dbAccessProvider.GetConnectionString();
        var serverVersion = _dbAccessProvider.GetServerVersion();
        optionsBuilder.UseMySql(connectionString, serverVersion);
    }
    
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _mediator.DispatchDomainEvents(this);

        return await base.SaveChangesAsync(cancellationToken);
    }
}