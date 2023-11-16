using FluentValidation;

namespace Application.MediaItems.Commands.Common;

public class CreateBaseMediaItemModelValidator : AbstractValidator<CreateBaseMediaItemModel>
{
    public CreateBaseMediaItemModelValidator()
    {
        RuleFor(model => model.Type)
            .Must(type =>
                type.Equals("image/jpeg") ||
                type.Equals("image/png")).WithMessage("Invalid content type");
    }
}