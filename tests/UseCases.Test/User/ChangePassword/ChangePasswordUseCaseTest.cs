using CommomTestUtilities.Encryption;
using CommomTestUtilities.Entities;
using CommomTestUtilities.LoggedUser;
using CommomTestUtilities.Repositories;
using CommomTestUtilities.Requests;
using Cookbook.Application.UseCases.User.ChangePassword;
using Cookbook.Communication.Requests;
using Cookbook.Exceptions;
using Cookbook.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.User.ChangePassword;

public class ChangePasswordUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, var password) = UserBuilder.Build();

        var request = ChangePasswordRequestBuilder.Build();
        request.Password = password;

        var useCase = CreateUseCase(user);

        Func<Task> act = async () => await useCase.Execute(request);

        await act.ShouldNotThrowAsync();

        var passwordEncripter = PasswordEncripterBuilder.Build();

        user.Password.ShouldBe(passwordEncripter.Encrypt(request.NewPassword));
    }

    [Fact]
    public async Task Error_NewPasswordEmpty()
    {
        (var user, var password) = UserBuilder.Build();

        var request = new ChangePasswordRequest
        {
            Password = password,
            NewPassword = string.Empty
        };

        var useCase = CreateUseCase(user);

        var ex = await Should.ThrowAsync<ErrorOnValidationException>(async () =>
        {
            await useCase.Execute(request);
        });

        ex.ErrorMessages.Count.ShouldBe(1);
        ex.ErrorMessages.ShouldContain(ResourceMessagesException.PASSWORD_EMPTY);

        var passwordEncripter = PasswordEncripterBuilder.Build();

        user.Password.ShouldBe(passwordEncripter.Encrypt(password));
    }

    [Fact]
    public async Task Error_CurrentPasswordDifferent()
    {
        (var user, var password) = UserBuilder.Build();

        var request = ChangePasswordRequestBuilder.Build();

        var useCase = CreateUseCase(user);

        var ex = await Should.ThrowAsync<ErrorOnValidationException>(async () =>
        {
            await useCase.Execute(request);
        });

        ex.ErrorMessages.Count.ShouldBe(1);
        ex.ErrorMessages.ShouldContain(ResourceMessagesException.PASSWORD_DIFFERENT_CURRENT_PASSWORD);

        var passwordEncripter = PasswordEncripterBuilder.Build();

        user.Password.ShouldBe(passwordEncripter.Encrypt(password));
    }

    private static ChangePasswordUseCase CreateUseCase(Cookbook.Domain.Entities.User user)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var userReadOnlyRepository = new UserReadOnlyRepositoryBuilder().GetById(user).Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var passwordEncripter = PasswordEncripterBuilder.Build();

        return new ChangePasswordUseCase(loggedUser, userReadOnlyRepository, unitOfWork, passwordEncripter);
    }
}
