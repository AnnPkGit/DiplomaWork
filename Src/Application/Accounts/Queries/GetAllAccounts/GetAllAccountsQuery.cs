using Application.Common.Interfaces;
using Domain.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Accounts.Queries.GetAllAccounts;

public record GetAllAccountsQuery : IRequest<IEnumerable<Account>>;

public class GetAllAccountQueryHandler : IRequestHandler<GetAllAccountsQuery, IEnumerable<Account>>
{
    private readonly IApplicationDbContext _context;

    public GetAllAccountQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Account>> Handle(GetAllAccountsQuery request, CancellationToken token)
    {
        return await _context.Accounts.ToListAsync(token);
    }
}