using System.Text.RegularExpressions;
using App.Repository;
using AutoMapper;
using Domain.Entity;
using Infrastructure.DbAccess.EfDbContext;
using Infrastructure.DbAccess.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DbAccess.Repository;

public class UserRepository : IUserRepository
{
    private readonly DataContext _dbContext;
    private readonly IMapper _mapper;

    public UserRepository(DataContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;

    }

    public async Task<bool> IsEmailUniqueAsync(string email)
    {
        // Проверка уникальности Email
        return await _dbContext.Users.AllAsync(u => u.Email != email);
    }

    public async Task<bool> IsLoginUniqueAsync(string login)
    {
        // Проверка уникальности Login
        return await _dbContext.Users.AllAsync(u => u.Login != login);
    }

    public Task<bool> IsPasswordStrongAsync(string password)
    {
       // Проверка сложности pass
        return Task.Run(() =>
        {
            if (!Regex.IsMatch(password, @"\p{Lu}"))
                return false;
            if (!Regex.IsMatch(password, @"\p{Ll}"))
                return false;
            if (!Regex.IsMatch(password, @"\d"))
                return false;
            if (!Regex.IsMatch(password, @"[^\p{L}\p{N}]"))
                return false;
            if (password.Length < 8)
                return false;

            return true;
        });
    }

    public async Task AddUserAsync(User user)
    {
        // Конвертируем User в UserEntity и добавляем в контекст базы данных
        var userEntity = _mapper.Map<UserEntity>(user);
        await _dbContext.Users.AddAsync(userEntity);
    }
}