namespace Application.Common.Exceptions;

public class ValidationException : Exception
{
    public ValidationException(string message)
        : base(message)
    {
        Errors = new Dictionary<string, string[]>();
    }
    public ValidationException()
        : base("One or more validation failures have occurred.")
    {
        Errors = new Dictionary<string, string[]>();
    }
    
    public IDictionary<string, string[]> Errors { get; }
}