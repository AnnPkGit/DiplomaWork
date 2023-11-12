using Application.MediaItems.Commands.Common;
using FluentValidation;

namespace Application.MediaItems.Commands.CreateToastMediaItem;

public class CreateToastMediaItemCommandValidator : AbstractValidator<CreateToastMediaItemCommand>
{
    public CreateToastMediaItemCommandValidator()
    {
        Include(new CreateBaseMediaItemModelValidator());
    }
}