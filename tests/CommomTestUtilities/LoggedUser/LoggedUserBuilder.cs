using Cookbook.Domain.Entities;
using Cookbook.Domain.Services.LoggedUser;
using Moq;

namespace CommomTestUtilities.LoggedUser;

public class LoggedUserBuilder
{
    public static ILoggedUser Build(User user)
    {
        var mock = new Mock<ILoggedUser>();

        mock.Setup(x => x.User()).ReturnsAsync(user);

        return mock.Object;
    }
}