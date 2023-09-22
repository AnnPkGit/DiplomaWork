using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Accounts.Queries.GetAccountByLogin;

public record GetAccountByLoginQuery(string Login) : IRequest<Account>;

public class GetAccountByLoginQueryHandler : IRequestHandler<GetAccountByLoginQuery, Account>
{
    private readonly IApplicationDbContext _context;

    public GetAccountByLoginQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Account> Handle(GetAccountByLoginQuery request, CancellationToken token)
    {
        var account = await _context.Accounts
            .SingleOrDefaultAsync(a => a.Login == request.Login, token);
        
        if (account == null)
            throw new NotFoundException(nameof(Account), request.Login);

        return account;
    }
}