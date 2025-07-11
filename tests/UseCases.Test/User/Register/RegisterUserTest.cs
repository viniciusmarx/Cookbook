using CommomTestUtilities.Encryption;
using CommomTestUtilities.Mapper;
using CommomTestUtilities.Repositories;
using CommomTestUtilities.Requests;
using Cookbook.Application.UseCases.User.Register;
using Shouldly;

namespace UseCases.Test.User.Register;

public class RegisterUserTest
{
    [Fact]
    public async Task Success()
    {
        var request = RegisterUserRequestBuilder.Build();

        var useCase = CreateUseCase();

        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.Name.ShouldBe(request.Name);
    }

    private RegisterUser CreateUseCase()
    {
        var writeRepository = UserWriteOnlyRepositoryBuilder.Build();
        var readRepository = new UserReadOnlyRepositoryBuilder().Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var mapper = MapperBuilder.Build();
        var passwordEncripter = PasswordEncripterBuilder.Build();

        return new RegisterUser(writeRepository, readRepository, unitOfWork, mapper, passwordEncripter);
    }
}
