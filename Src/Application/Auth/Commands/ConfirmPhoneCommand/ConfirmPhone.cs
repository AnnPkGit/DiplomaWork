using Application.Common.Interfaces;
using MediatR;

namespace Application.Auth.Commands.ConfirmPhoneCommand

{
    public record ConfirmPhoneCommand(string VerificationCode) : IRequest<bool>;

    public class ConfirmPhoneCommandHandler : IRequestHandler<ConfirmPhoneCommand, bool>
    {
        private readonly IConfirmPhoneService _confirmPhoneService;

        public ConfirmPhoneCommandHandler(IConfirmPhoneService confirmPhoneService)
        {
            _confirmPhoneService = confirmPhoneService;
        }

        public async Task<bool> Handle(ConfirmPhoneCommand request, CancellationToken cancellationToken)
        {
            return await _confirmPhoneService.ConfirmPhone(request.VerificationCode);
        }
    }
}