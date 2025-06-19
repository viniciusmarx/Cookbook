namespace Cookbook.Domain.Interfaces.Repositories.User;

public interface IUserWriteOnlyRepository
{
    public Task Add(Entities.User user);
}
