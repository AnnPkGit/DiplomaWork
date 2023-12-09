namespace Application.BaseToasts.Queries.Models;

public class DeletedBaseToastBriefDto : BaseToastBriefDto
{
    public DeletedBaseToastBriefDto()
    {
        Id = 0;
        Author = null;
        Type = "DeletedBaseToast";
    }
}