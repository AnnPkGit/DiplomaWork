namespace Domain.Common;

public class Result
{
    public bool IsSuccessful { get; private set; }

    public string? ErrorMessage { get; private set; }

    public static Result Successful()
    {
        return new Result(true, null);
    }

    public static Result Failed(string errorMessage)
    {
        return new Result(false, errorMessage);
    }

    private Result(bool isSuccessful, string? errorMessage = null)
    {
        IsSuccessful = isSuccessful;
        ErrorMessage = errorMessage;
    }
}