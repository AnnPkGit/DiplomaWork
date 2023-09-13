using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entity;
using MediatR;

namespace Application.Users.Commands.UpdateUserEmail;

public record UpdateUserEmailCommand(string NewEmail) : IRequest;

public class UpdateUserEmailCommandHandler : IRequestHandler<UpdateUserEmailCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IEmailService _emailService;

    public UpdateUserEmailCommandHandler(
        ICurrentUserService currentUserService,
        IApplicationDbContext context,
        IEmailService emailService)
    {
        _currentUserService = currentUserService;
        _context = context;
        _emailService = emailService;
    }

    public async Task Handle(UpdateUserEmailCommand request, CancellationToken token)
    {
        var userId = _currentUserService.UserId;
        var user = await _context.Users.FindAsync(new object?[] { userId }, token);

        if (user == null)
            throw new NotFoundException(nameof(User), userId);

        await _emailService.SendEmailAsync(new MailRequest
        {
            Subject = "no-reply",
            ToEmail = request.NewEmail,
            Body = "Test Message"
        }, token);
            
        user.Email = request.NewEmail;
        
        await _context.SaveChangesAsync(token);
    }
}