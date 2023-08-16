﻿using App.Users.Login;
using Domain.Common;
using Domain.Entity;

namespace App.Service;

public interface IUserService
{
    Task<Result> AddUserAsync(User user);
    Task<Result<LoginResponse>> LoginUserAsync(LoginRequest user);
    Task<IEnumerable<User>> GetAllUsersAsync();
}