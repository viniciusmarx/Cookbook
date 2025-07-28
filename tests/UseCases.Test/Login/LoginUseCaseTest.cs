using CommomTestUtilities.Encryption;
using CommomTestUtilities.Entities;
using CommomTestUtilities.Repositories;
using CommomTestUtilities.Requests;
using CommomTestUtilities.Tokens;
using Cookbook.Application.UseCases.Login;
using Cookbook.Communication.Requests;
using Cookbook.Exceptions;
using Cookbook.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.Login;

public class LoginUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, var password) = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        var result = await useCase.Execute(new LoginRequest
        {
            Email = user.Email,
            Password = password
        });

        result.ShouldNotBeNull();
        result.Tokens.ShouldNotBeNull();
        result.Name.ShouldBe(user.Name);
        result.Tokens.AccessToken.ShouldNotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Error_InvalidUser()
    {
        var request = LoginRequestBuilder.Build();

        var useCase = CreateUseCase();

        var ex = await Should.ThrowAsync<InvalidLoginException>(async () =>
        {
            await useCase.Execute(request);
        });

        ex.Message.Contains(ResourceMessagesException.EMAIL_OR_PASSWORD_INVALID).ShouldBeTrue();
    }

    private static LoginUseCase CreateUseCase(Cookbook.Domain.Entities.User? user = null)
    {
        var passwordEncripter = PasswordEncripterBuilder.Build();
        var userReadOnlyRepositoryBuilder = new UserReadOnlyRepositoryBuilder();
        var accessTokenGenerator = JwtTokenGeneratorBuilder.Build();

        if (user is not null)
            userReadOnlyRepositoryBuilder.GetByEmailAndPassword(user);

        return new LoginUseCase(userReadOnlyRepositoryBuilder.Build(), passwordEncripter, accessTokenGenerator);
    }
}
