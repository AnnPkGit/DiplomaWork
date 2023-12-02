using Application.Accounts.Queries.Models;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Accounts.Queries.GetAccountById;

public record GetAccountByIdQuery(int Id) : IRequest<AccountDto>;

public class GetAccountByIdQueryHandler : IRequestHandler<GetAccountByIdQuery, AccountDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAccountByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<AccountDto> Handle(GetAccountByIdQuery request, CancellationToken token)
    {
        var account = await _context.Accounts
            .Include(a => a.Follows)
            .Include(a => a.Followers)
            .Include(a => a.Banner)
            .AsSingleQuery()
            .SingleOrDefaultAsync(a => a.Id == request.Id, token);
        
        if (account == null)
            throw new NotFoundException(nameof(Account), request.Id);
        
        return _mapper.Map<AccountDto>(account);
    }
}