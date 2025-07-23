namespace Cookbook.Domain.Interfaces.Repositories.User;

public interface IUserReadOnlyRepository
{
    public Task<bool> ExistActiveUserWithEmail(string email);
    public Task<Entities.User?> GetByEmailAndPasswsord(string email, string password);
}