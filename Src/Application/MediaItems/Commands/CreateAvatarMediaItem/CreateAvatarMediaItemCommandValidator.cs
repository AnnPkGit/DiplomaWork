using Application.MediaItems.Commands.Common;
using FluentValidation;

namespace Application.MediaItems.Commands.CreateAvatarMediaItem;

public class CreateAvatarMediaItemCommandValidator : AbstractValidator<CreateAvatarMediaItemCommand>
{
    public CreateAvatarMediaItemCommandValidator()
    {
        Include(new CreateBaseMediaItemModelValidator());
    }
}