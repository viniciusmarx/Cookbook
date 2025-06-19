namespace Cookbook.Domain.Interfaces.Repositories;
public interface IUnitOfWork
{
    public Task Commit();
}
