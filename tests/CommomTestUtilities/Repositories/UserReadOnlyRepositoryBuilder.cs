using Cookbook.Domain.Interfaces.Repositories.User;
using Moq;

namespace CommomTestUtilities.Repositories;

public class UserReadOnlyRepositoryBuilder
{
    private readonly Mock<IUserReadOnlyRepository> _repository = new();

    public IUserReadOnlyRepository Build() => _repository.Object;
}
