using Application.BaseToasts.Queries.Models;
using Application.Common.Constants;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Common;

namespace Application.Common.Mappings.Actions;

public class SetBaseToastWithContentDtoAction : IMappingAction<BaseToastWithContent, BaseToastWithContentBriefDto>, IMappingAction<BaseToastWithContent, BaseToastWithContentDto>
{
    private readonly ICurrentUserService _userService;

    public SetBaseToastWithContentDtoAction(ICurrentUserService userService)
    {
        _userService = userService;
    }

    public void Process(BaseToastWithContent source, BaseToastWithContentBriefDto destination, ResolutionContext context)
    {
        var accountId = _userService.Id;
        if (accountId != UserDefaultValues.Id)
        {
            destination.YouReacted = source.Reactions.Any(reaction => reaction.AuthorId == accountId);
            destination.YouReToasted = source.ReToasts.Any(reToast => reToast.AuthorId == accountId);
        }
        else
        {
            destination.YouReacted = false;
            destination.YouReToasted = false;
        }
    }

    public void Process(BaseToastWithContent source, BaseToastWithContentDto destination, ResolutionContext context)
    {
        var accountId = _userService.Id;
        if (accountId != UserDefaultValues.Id)
        {
            destination.YouReacted = source.Reactions.Any(reaction => reaction.AuthorId == accountId);
            destination.YouReToasted = source.ReToasts.Any(reToast => reToast.AuthorId == accountId);
        }
        else
        {
            destination.YouReacted = false;
            destination.YouReToasted = false;
        }
    }
}