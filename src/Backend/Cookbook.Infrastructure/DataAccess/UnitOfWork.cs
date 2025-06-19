using Cookbook.Domain.Interfaces.Repositories;

namespace Cookbook.Infrastructure.DataAccess;

public class UnitOfWork(CookbookDbContext dbContext) : IUnitOfWork
{
    private readonly CookbookDbContext _dbContext = dbContext;

    public async Task Commit() => await _dbContext.SaveChangesAsync();
}
