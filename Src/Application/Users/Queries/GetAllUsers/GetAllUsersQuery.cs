using Application.Common.Interfaces;
using Domain.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Queries.GetAllUsers;

public record GetAllUsersQuery : IRequest<IEnumerable<User>>;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<User>>
{
    private readonly IApplicationDbContext _context;

    public GetAllUsersQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> Handle(GetAllUsersQuery request, CancellationToken token)
    {
        return await _context.Users.ToListAsync(token);
    }
}