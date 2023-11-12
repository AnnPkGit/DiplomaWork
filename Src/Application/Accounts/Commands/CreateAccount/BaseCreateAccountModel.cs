namespace Application.Accounts.Commands.CreateAccount;

public abstract class BaseCreateAccountModel
{
    public string Login { get; init; } = string.Empty;
    public string? Name { get; init; }
}