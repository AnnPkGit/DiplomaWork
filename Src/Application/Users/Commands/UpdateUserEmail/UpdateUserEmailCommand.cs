using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Validators;
using Domain.Entity;
using MediatR;

namespace Application.Users.Commands.UpdateUserEmail;

public record UpdateUserEmailCommand(string NewEmail) : IRequest;

public class UpdateUserEmailCommandHandler : IRequestHandler<UpdateUserEmailCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserValidator _validator;

    public UpdateUserEmailCommandHandler(
        IUserValidator validator,
        ICurrentUserService currentUserService,
        IApplicationDbContext context)
    {
        _validator = validator;
        _currentUserService = currentUserService;
        _context = context;
    }

    public async Task Handle(UpdateUserEmailCommand request, CancellationToken token)
    {
        var userId = _currentUserService.UserId;
        var user = await _context.Users.FindAsync(new object?[] { userId }, token);

        if (user == null)
            throw new NotFoundException(nameof(User), userId);
        
        if (!await _validator.IsEmailUniqueAsync(request.NewEmail))
            throw new ValidationException("This email is already taken");
        
        // TODO: Implement phone number verification

        user.Email = request.NewEmail;
        
        var res = await _context.SaveChangesAsync(token);
        throw new NotImplementedException();
    }
}