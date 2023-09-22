using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Accounts.Queries.GetAccountById;

public record GetAccountByIdQuery(int Id) : IRequest<Account>;

public class GetAccountByIdQueryHandler : IRequestHandler<GetAccountByIdQuery, Account>
{
    private readonly IApplicationDbContext _context;

    public GetAccountByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Account> Handle(GetAccountByIdQuery request, CancellationToken token)
    {
        var account = await _context.Accounts.FindAsync(
            new object?[] { request.Id },
            token);
        
        if (account == null)
            throw new NotFoundException(nameof(Account), request.Id);

        return account;
    }
}