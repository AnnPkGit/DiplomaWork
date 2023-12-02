using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Security;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Accounts.Commands.UpdateAccountDetail;

[Authorize]
public record UpdateAccountDetailCommand : IRequest
{
    public string? Login { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Name { get; set; }
    public int? AvatarId { get; set; }
    public int? BannerId { get; set; }
    public string? Bio { get; set; }
}

public class UpdateAccountDetailCommandHandler : IRequestHandler<UpdateAccountDetailCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UpdateAccountDetailCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task Handle(UpdateAccountDetailCommand request, CancellationToken token)
    {
        var accountId = _currentUserService.Id;
        var entity = await _context.Accounts.FindAsync(
            new object?[] { accountId }, token);
        if (entity == null)
            throw new NotFoundException(nameof(Account), accountId);

        var newLogin = request.Login;
        if (!string.IsNullOrEmpty(newLogin))
            entity.Login = newLogin;
        
        var newName = request.Name;
        if (newName != null)
            entity.Name = newName;
        
        var newBirthDate = request.BirthDate;
        if (newBirthDate != default)
            entity.BirthDate = newBirthDate;
        
        var newBio = request.Bio;
        if (newName != null)
            entity.Bio = newBio;

        var newAvatar = request.AvatarId;
        if (newAvatar != null)
        {
            if (newAvatar == 0)
            {
                entity.AvatarId = null;
            }
            else
            {
                if (!await _context.AvatarMediaItems.AnyAsync(item => item.Id == newAvatar, token))
                {
                    throw new NotFoundException(nameof(AvatarMediaItem), newAvatar);
                }
                entity.AvatarId = newAvatar;
            }
        }
        
        var newBanner = request.BannerId;
        if (newBanner != null)
        {
            if (newBanner == 0)
            {
                entity.BannerId = null;
            }
            else
            {
                if (!await _context.BannerMediaItems.AnyAsync(item => item.Id == newBanner, token))
                {
                    throw new NotFoundException(nameof(BannerMediaItem), newBanner);
                }
                entity.BannerId = newBanner;
            }
        }
        
        await _context.SaveChangesAsync(token);
    }
}