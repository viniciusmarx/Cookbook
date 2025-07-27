using Cookbook.Domain.Entities;
using Cookbook.Domain.Repositories.User;
using Microsoft.EntityFrameworkCore;

namespace Cookbook.Infrastructure.DataAccess.Repositories;

public class UserRepository(CookbookDbContext dbContext) : IUserWriteOnlyRepository, IUserReadOnlyRepository
{
    private readonly CookbookDbContext _dbContext = dbContext;

    public async Task Add(User user) => await _dbContext.Users.AddAsync(user);

    public async Task<bool> ExistActiveUserWithEmail(string email) => await _dbContext.Users.AnyAsync(user => user.Email.Equals(email) && user.IsActive);

    public async Task<bool> ExistActiveUserWithIdentifier(Guid userIdentifier) => await _dbContext.Users.AnyAsync(user => user.UserIdentifier.Equals(userIdentifier) && user.IsActive);

    public async Task<User?> GetByEmailAndPasswsord(string email, string password)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.IsActive && user.Email.Equals(email) && user.Password.Equals(password));
    }
}
