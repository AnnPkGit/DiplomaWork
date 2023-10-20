namespace Domain.Constants;

public static class ToastType
{
    public static string Reply { get; private set; } = "reply";
    public static string Quote { get; private set; } = "quote";
    public static string Toast { get; private set; } = "toast";
    public static string ReToast { get; private set; } = "retoast";
}