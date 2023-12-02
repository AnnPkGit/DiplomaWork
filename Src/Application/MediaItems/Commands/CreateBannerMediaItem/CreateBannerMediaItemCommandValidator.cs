using Application.MediaItems.Commands.Common;
using FluentValidation;

namespace Application.MediaItems.Commands.CreateBannerMediaItem;

public class CreateBannerMediaItemCommandValidator : AbstractValidator<CreateBannerMediaItemCommand>
{
    public CreateBannerMediaItemCommandValidator()
    {
        Include(new CreateBaseMediaItemModelValidator());
    }
}