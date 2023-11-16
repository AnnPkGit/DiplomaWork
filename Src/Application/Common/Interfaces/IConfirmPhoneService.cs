namespace Application.Common.Interfaces;

public interface IConfirmPhoneService
{
    Task<bool> ConfirmPhone(string verificationCode);
    
   
}
public enum ConfirmPhoneResult
{
    Success,
    InvalidCode
}
