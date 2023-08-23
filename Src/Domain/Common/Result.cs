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

public class Result<T>
{
    public T? Value { get; private set; }
    public bool IsSuccessful { get; private set; }

    public string? ErrorMessage { get; private set; }

    public static Result<T> Successful(T value)
    {
        return new Result<T>(true, value, null);
    }

    public static Result<T> Failed(string errorMessage)
    {
        return new Result<T>(false, errorMessage);
    }
    private Result(bool isSuccessful, string? errorMessage = null)
    {
        IsSuccessful = isSuccessful;
        ErrorMessage = errorMessage;
    }
    private Result(bool isSuccessful, T? value, string? errorMessage = null)
    {
        IsSuccessful = isSuccessful;
        ErrorMessage = errorMessage;
        Value = value;
    }
}