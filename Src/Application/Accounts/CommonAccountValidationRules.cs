using System.Diagnostics.CodeAnalysis;
using Application.Common.Interfaces;
using FluentValidation;

namespace Application.Accounts;

public static class CommonAccountValidationRules
{
    public const int MinimumLoginLenght = 4;
    public const int MaximumLoginLenght = 15;
    
    public const int MinimumNameLenght = 1;
    public const int MaximumNameLenght = 20;
    
    public const int MaximumBioLenght = 160;
    
    [StringSyntax("Regex")]
    public const string LoginRegex = @"^(?=.*[A-Za-z0-9]$)[A-Za-z][A-Za-z\d._-]{0,}$";
    public const string LoginRegexErrStr = "Login must start with a letter and end with a letter or number";
    
    public static readonly string NameLengthErrMsg = $"Name must have between {MinimumNameLenght} and {MaximumNameLenght} characters";
    public static IRuleBuilderOptions<T, string> BeUniqueLogin<T>(this IRuleBuilder<T, string> ruleBuilder, IApplicationDbContext context)
    {
        return ruleBuilder.Must(login => context.Accounts.All(e => e.Login != login));
    }
}