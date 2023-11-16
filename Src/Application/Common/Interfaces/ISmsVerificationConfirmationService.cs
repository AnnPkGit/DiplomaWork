namespace Application.Common.Interfaces;

public interface ISmsVerificationConfirmationService
{
    Task<bool> ConfirmSmsVerificationCode(string code);
}