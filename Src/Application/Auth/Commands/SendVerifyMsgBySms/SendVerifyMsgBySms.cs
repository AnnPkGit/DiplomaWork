using Application.Common.Interfaces;

using MediatR;


namespace Application.Auth.Commands.SendVerifyMsgBySms;

public record SendVerifyMsgBySms();

public class SendSmsCommandHandler : IRequestHandler<SendSmsCommand>
{
    private readonly ISmsVerify _smsVerifyService;


    public SendSmsCommandHandler(ISmsVerify smsVerifyService)
    {
        _smsVerifyService = smsVerifyService;
       
    }

    public async Task Handle(SendSmsCommand request, CancellationToken cancellationToken)
    {
        // Вызов метода для отправки SMS из сервиса
        await _smsVerifyService.SendSms();
    }
}


