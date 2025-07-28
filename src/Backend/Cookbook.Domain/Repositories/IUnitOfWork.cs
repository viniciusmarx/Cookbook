namespace Cookbook.Domain.Repositories;
public interface IUnitOfWork
{
    public Task Commit();
}
