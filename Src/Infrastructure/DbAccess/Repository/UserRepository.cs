using App.Repository;
using AutoMapper;
using Domain.Entity;
using Infrastructure.DbAccess.EfDbContext;
using Infrastructure.DbAccess.Entity;

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

    public IQueryable<User> GetAll()
    {
        return _mapper.ProjectTo<User>(_dbContext.Users);
    }

    public async Task AddUserAsync(User user)
    {
        try
        {
            // Конвертируем User в UserEntity и добавляем в контекст базы данных
            var userEntity = _mapper.Map<UserEntity>(user);
            await _dbContext.Users.AddAsync(userEntity);

            // Сохраняем изменения в базе данных
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner exception: {ex.InnerException.Message}  " + ex);
            }
       
            throw; 
        }
    }
}