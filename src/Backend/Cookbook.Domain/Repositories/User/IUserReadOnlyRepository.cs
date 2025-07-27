namespace Cookbook.Domain.Repositories.User;

public interface IUserReadOnlyRepository
{
    public Task<bool> ExistActiveUserWithEmail(string email);
    public Task<Entities.User?> GetByEmailAndPasswsord(string email, string password);
    public Task<bool> ExistActiveUserWithIdentifier(Guid userIdentifier);
}