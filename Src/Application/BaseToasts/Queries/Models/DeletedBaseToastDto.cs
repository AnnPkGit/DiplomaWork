namespace Application.BaseToasts.Queries.Models;

public class DeletedBaseToastDto : BaseToastDto
{
    public DeletedBaseToastDto()
    {
        Id = 0;
        Author = null;
        Type = "DeletedBaseToast";
    }
}