using Cookbook.Domain.Entities;

namespace Cookbook.Domain.Services.LoggedUser;

public interface ILoggedUser
{
    public Task<User> User();
}
