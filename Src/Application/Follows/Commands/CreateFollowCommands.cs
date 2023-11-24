using Application.Common.Interfaces;
using Application.Common.Security;
using Application.Follows.Queries.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Follows.Commands;

[Authorize]
public record CreateFollowCommand( int FollowingId) : IRequest<FollowDto>;

public class CreateFollowCommandHandler : IRequestHandler<CreateFollowCommand, FollowDto>
{
    private readonly ICurrentUserService _userService;
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateFollowCommandHandler(
        ICurrentUserService userService,
        IApplicationDbContext context,
        IMapper mapper)
    {
        _userService = userService;
        _context = context;
        _mapper = mapper;
    }

    public async Task<FollowDto> Handle(CreateFollowCommand request, CancellationToken cancellationToken)
    {
        var followerId = _userService.Id;
        var followingId = request.FollowingId;

        // Проверка на существование подписки
        var existingFollow = await _context.Follows
            .FirstOrDefaultAsync(f => f.AccountId == followerId && f.ToAccountId == followingId, cancellationToken);

        if (existingFollow != null)
        {
            // Возвращаем существующую подписку, не создавая новую
            return _mapper.Map<FollowDto>(existingFollow);
        }

        // Создание новой подписки
        var newFollow = new Follow
        {
            AccountId = followerId,
            ToAccountId = followingId,
            DateOfFollow = DateTime.Now
        };

        await _context.Follows.AddAsync(newFollow, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<FollowDto>(newFollow);
    }
}