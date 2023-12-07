namespace Application.Common.Configurations.Options;

public class EmailConfirmationSenderOptions
{
    public const string LINK_CONTENT = "#LinkContent";
    public string ConfirmPageUrl { get; set; } = string.Empty;
    public string LinkContent { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;

    public bool CheckRequiredValues()
    {
        return ConfirmPageUrl != string.Empty &&
               LinkContent != string.Empty &&
               Body.Contains(LINK_CONTENT);
    }
}