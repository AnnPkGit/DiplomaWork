using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using static Domain.Entities.MuteNotificationOption;
namespace Application.Common.Services;

public class MuteNotificationOptionsChecker : IMuteNotificationOptionsChecker
{
    private readonly IApplicationDbContext _context;
    private readonly IDateTime _dateTime;

    public MuteNotificationOptionsChecker(IApplicationDbContext context, IDateTime dateTime)
    {
        _context = context;
        _dateTime = dateTime;
    }

    public async Task<bool> CheckMuteOptions(int? fromAccountId, int? toAccountId, CancellationToken cancellationToken = default)
    {
        if (fromAccountId == null || toAccountId == null)
        {
            return false;
        }
        var fromAccountIdValue = fromAccountId.Value;
        var toAccountIdValue = toAccountId.Value;
        var toUser = FindUser(toAccountIdValue);
        var fromUser = FindUser(fromAccountIdValue);

        var toUserMuteOptions = await _context.Users.Entry(toUser)
            .Collection(user => user.MuteNotificationOptions)
            .Query().AsSingleQuery().ToArrayAsync(cancellationToken);

        foreach (var muteOption in toUserMuteOptions)
        {
            if (muteOption.Equals(YouDoNotFollow))
            {
                if (CheckYouDoNotFollowOption(fromAccountIdValue, toAccountIdValue))
                    continue;

                return false;
            }
            if (muteOption.Equals(WhoDoNotFollowYou))
            {
                if (CheckWhoDoNotFollowYouOption(fromAccountIdValue, toAccountIdValue))
                    continue;

                return false;
            }
            if (muteOption.Equals(WithANewAccount))
            {
                if (CheckWithANewAccountOption(fromAccountIdValue))
                    continue;

                return false;
            }
            if (muteOption.Equals(WhoHaveDefaultProfilePhoto))
            {
                if (CheckWhoHaveDefaultProfilePhotoOption(fromAccountIdValue))
                    continue;

                return false;
            }
            if (muteOption.Equals(WhoHaveNotConfirmedTheirEmail))
            {
                if (CheckWhoHaveNotConfirmedTheirEmail(fromUser.Id))
                    continue;

                return false;
            }
            if (muteOption.Equals(WhoHaveNotConfirmedTheirPhoneNumber))
            {
                if (CheckWhoHaveNotConfirmedTheirPhoneNumber(fromUser.Id))
                    continue;

                return false;
            }
        }

        return true;
    }
    
    private (Account fromAccount, Account toAccount) FindAccounts(int fromAccountId, int toAccountId)
    {
        var fromAccount = FindAccount(fromAccountId);
        var toAccount = FindAccount(toAccountId);

        return (fromAccount, toAccount);
    }

    private Account FindAccount(int accountId)
    {
        var account = _context.Accounts.Find(accountId);
        if (account == null)
        {
            throw new NotFoundException(nameof(Account), accountId);
        }
        return account;
    }
    
    private User FindUser(int userId)
    {
        var user = _context.Users.Find(userId);
        if (user == null)
        {
            throw new NotFoundException(nameof(User), userId);
        }
        return user;
    }

    private bool CheckYouDoNotFollowOption(int fromAccountId, int toAccountId)
    {
        var accounts = FindAccounts(fromAccountId, toAccountId);
        var fromAccount = accounts.fromAccount;
        _context.Accounts.Entry(fromAccount)
            .Collection(account => account.Followers)
            .Load();
        
        return fromAccount.Followers.Any(follow => follow.FollowFromId == toAccountId);
    }
    
    private bool CheckWhoDoNotFollowYouOption(int fromAccountId, int toAccountId)
    {
        var accounts = FindAccounts(fromAccountId, toAccountId);
        var fromAccount = accounts.fromAccount;
        _context.Accounts.Entry(fromAccount)
            .Collection(account => account.Follows)
            .Load();
        
        return fromAccount.Follows.Any(follow => follow.FollowToId == toAccountId);
    }
    
    private bool CheckWithANewAccountOption(int fromAccountId)
    {
        var fromAccount = FindAccount(fromAccountId);
        
        const int days = 5;
        var now = _dateTime.Now;
        var minTimeInterval = TimeSpan.FromDays(days);
        var timeInterval = now.Subtract(fromAccount.Created);

        return timeInterval > minTimeInterval;
    }
    
    private bool CheckWhoHaveDefaultProfilePhotoOption(int fromAccountId)
    {
        var fromAccount = FindAccount(fromAccountId);

        return fromAccount.AvatarId != null;
    }

    private bool CheckWhoHaveNotConfirmedTheirEmail(int fromUserId)
    {
        var fromUser = FindUser(fromUserId);

        return fromUser.EmailVerified;
    }
    
    private bool CheckWhoHaveNotConfirmedTheirPhoneNumber(int fromUserId)
    {
        var fromUser = FindUser(fromUserId);

        return fromUser.PhoneVerified;
    }
}