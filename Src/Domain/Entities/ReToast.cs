namespace Domain.Entities;

public class ReToast : BaseToast
{
    public ReToast()
    {
    }

    public ReToast(int authorId, int toastWithContentId) : base(authorId, typeof(ReToast))
    {
        ToastWithContentId = toastWithContentId;
    }

    public int? ToastWithContentId { get; set; }
    public BaseToastWithContent? ToastWithContent { get; set; } = null!;
}