using Application.Accounts.Queries.Models;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Accounts.Queries.GetAccountsWithPaginationQuery;

public class GetAccountsWithPaginationQuery : IRequest<PaginatedList<AccountBriefDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetAccountsWithPaginationQueryHandler : IRequestHandler<GetAccountsWithPaginationQuery, PaginatedList<AccountBriefDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAccountsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<AccountBriefDto>> Handle(GetAccountsWithPaginationQuery request, CancellationToken token)
    {
        return await _context.Accounts
            .IgnoreAutoIncludes()
            .OrderBy(account => account.Login)
            .ProjectTo<AccountBriefDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}