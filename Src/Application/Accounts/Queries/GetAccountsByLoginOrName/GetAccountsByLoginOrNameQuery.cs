using Application.Accounts.Queries.Models;
using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Accounts.Queries.GetAccountsByLoginOrName;

public class GetAccountsByLoginOrNameQuery : IRequest<IEnumerable<AccountBriefDto>>
{
    public string Text { get; init; } = string.Empty;
}

public class GetAccountsByLoginOrNameQueryHandler : IRequestHandler<GetAccountsByLoginOrNameQuery, IEnumerable<AccountBriefDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAccountsByLoginOrNameQueryHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AccountBriefDto>> Handle(GetAccountsByLoginOrNameQuery request, CancellationToken cancellationToken)
    {
        const int pageSize = 10;
        
        var accounts = await _context.Accounts
            .OrderBy(account => account.Login)
            .Where(account =>
                account.Login.Contains(request.Text) ||
                (account.Name != null && account.Name.Contains(request.Text)))
            .Take(pageSize).ToArrayAsync(cancellationToken);

        var accountDtos = accounts.Select(account => _mapper.Map<AccountBriefDto>(account));
        
        return accountDtos;
    }
}