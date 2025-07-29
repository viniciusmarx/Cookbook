using CommomTestUtilities.Encryption;
using CommomTestUtilities.Mapper;
using CommomTestUtilities.Repositories;
using CommomTestUtilities.Requests;
using CommomTestUtilities.Tokens;
using Cookbook.Application.UseCases.User.Register;
using Cookbook.Exceptions;
using Cookbook.Exceptions.ExceptionsBase;
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
        result.Tokens.ShouldNotBeNull();
        result.Tokens.AccessToken.ShouldNotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Error_NameEmpty()
    {
        var request = RegisterUserRequestBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreateUseCase();

        var ex = await Should.ThrowAsync<ErrorOnValidationException>(async () =>
        {
            await useCase.Execute(request);
        });

        ex.ErrorMessages.Count.ShouldBe(1);
        ex.ErrorMessages.ShouldContain(ResourceMessagesException.NAME_EMPTY);
    }

    [Fact]
    public async Task Error_EmailAlreadyRegistered()
    {
        var request = RegisterUserRequestBuilder.Build();

        var useCase = CreateUseCase(request.Email);

        var ex = await Should.ThrowAsync<ErrorOnValidationException>(async () =>
        {
            await useCase.Execute(request);
        });

        ex.ErrorMessages.Count.ShouldBe(1);
        ex.ErrorMessages.Contains(ResourceMessagesException.EMAIL_ALREADY_REGISTERED).ShouldBeTrue();
    }

    private static RegisterUser CreateUseCase(string? email = null)
    {
        var writeRepository = UserWriteOnlyRepositoryBuilder.Build();
        var readRepositoryBuilder = new UserReadOnlyRepositoryBuilder();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var mapper = MapperBuilder.Build();
        var passwordEncripter = PasswordEncripterBuilder.Build();
        var accessTokenGenerator = JwtTokenGeneratorBuilder.Build();

        if (!string.IsNullOrEmpty(email))
        {
            readRepositoryBuilder.ExistActiveUserWithEmail(email);
        }

        return new RegisterUser(writeRepository, readRepositoryBuilder.Build(), unitOfWork, mapper, passwordEncripter, accessTokenGenerator);
    }
}
