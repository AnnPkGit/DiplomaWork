using Application.Accounts.Queries.Models;
using Application.Common.Constants;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mappings.Actions;

public class SetAccountDtoAction : IMappingAction<Account, AccountDto>
{
    private readonly ICurrentUserService _userService;

    public SetAccountDtoAction(ICurrentUserService userService)
    {
        _userService = userService;
    }

    public void Process(Account source, AccountDto destination, ResolutionContext context)
    {
        var accountId = _userService.Id;
        destination.YouFollow = accountId != UserDefaultValues.Id && source.Followers
            .Any(follow => follow.FollowFromId == accountId);
    }
}