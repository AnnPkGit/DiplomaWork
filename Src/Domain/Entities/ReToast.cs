namespace Domain.Entities;

public class ReToast
{
    public ReToast()
    {
    }
    
    public ReToast(int toastId, int accountId, DateTime created)
    {
        ToastId = toastId;
        AccountId = accountId;
        Created = created;
    }

    public int ToastId { get; set; }
    public int AccountId { get; set; }
    public DateTime Created { get; set; }
}