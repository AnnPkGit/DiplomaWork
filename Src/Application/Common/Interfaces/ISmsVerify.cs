using MediatR;

namespace Application.Common.Interfaces;

public class SendSmsCommand : IRequest
{
    public string Message { get; set; }
    public string Sender { get; set; }
    public string[] Recipients { get; set; }
}

public interface ISmsVerify
{
    Task SendSms();
    
}