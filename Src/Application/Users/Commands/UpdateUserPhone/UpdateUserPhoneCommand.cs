using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Validators;
using MediatR;

namespace Application.Users.Commands.UpdateUserPhone;

public record UpdateUserPhoneCommand(string NewPhone) : IRequest;

public class UpdateUserPhoneCommandHandler : IRequestHandler<UpdateUserPhoneCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserValidator _validator;

    public UpdateUserPhoneCommandHandler(
        ICurrentUserService currentUserService,
        IApplicationDbContext context,
        IUserValidator validator)
    {
        _currentUserService = currentUserService;
        _context = context;
        _validator = validator;
    }

    public async Task Handle(UpdateUserPhoneCommand request, CancellationToken token)
    {
        var userId = _currentUserService.UserId;
        var user = await _context.Users.FindAsync(new object?[] { userId }, token);

        if (user == null)
            throw new NotFoundException("User", userId);

        if (!await _validator.IsPhoneUniqueAsync(request.NewPhone, token))
            throw new ValidationException("This phone is already taken");
        
        // TODO: Implement phone number verification
        
        user.Phone = request.NewPhone;
        
        await _context.SaveChangesAsync(token);
    }
}