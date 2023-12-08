using MediatR;

namespace Application.Common.Interfaces;

public class SendSmsCommand : IRequest
{
    public string Message { get; set; } = string.Empty;
    public string Sender { get; set; } = string.Empty;
    public string[] Recipients { get; set; } = Array.Empty<string>();
}

public interface ISmsVerify
{
    Task SendSms();
    
}