using System.Threading.Tasks;
using Application.Common.Interfaces;
using Infrastructure.Persistence; 
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class ConfirmPhoneService : IConfirmPhoneService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public ConfirmPhoneService(ApplicationDbContext dbContext, ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
        }

        public async Task<bool> ConfirmPhone(string verificationCode)
        {
            // Получение id текущего пользователя
            var currentUserId = _currentUserService.Id;

            // Поиск юзера по id и проверка кода подтверждения
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == currentUserId && u.PhoneVerifyCode == int.Parse(verificationCode));

            if (user != null)
            {
                user.PhoneVerified = true;
                user.PhoneVerifyCode = null;
                await _dbContext.SaveChangesAsync();

                return true;
            }

            return false;
        }
    }
}