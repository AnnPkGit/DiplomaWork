using System.Diagnostics.CodeAnalysis;
using Application.Common.Interfaces;
using FluentValidation;

namespace Application.Users;

public static class CommonUserValidationRules
{
    public const int MinimumPasswordLenght = 8;
    public const int MaximumPasswordLenght = 100;
    
    public const int PhoneLenght = 12;
    
    [StringSyntax("Regex")]
    public const string EmailRegex = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
    [StringSyntax("Regex")]
    public const string PasswordRegex = @"^(?=.*\p{Lu})(?=.*\p{Ll})(?=.*\d)(?=.*[^\p{L}\p{N}]).{0,}$";

    public const string PassRegexErrStr = "The password must contain one digit, one non alphanumeric character, one letter A-Z and one a-z";

    public static readonly string PassLenghtErrStr = $"The Password must be at least {MinimumPasswordLenght} and at max {MaximumPasswordLenght} characters long.";

    public static IRuleBuilderOptions<T, string> BeUniqueEmail<T>(this IRuleBuilder<T, string> ruleBuilder, IApplicationDbContext context)
    {
        return ruleBuilder.Must(email => context.Users.All(e => e.Email != email));
    }
}