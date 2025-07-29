using CommomTestUtilities.Entities;
using CommomTestUtilities.LoggedUser;
using CommomTestUtilities.Repositories;
using CommomTestUtilities.Requests;
using Cookbook.Application.UseCases.User.Update;
using Cookbook.Exceptions;
using Cookbook.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.User.Update;

public class UpdateUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, _) = UserBuilder.Build();

        var request = UpdateUserRequestBuilder.Build();

        var useCase = CreateUseCase(user);

        Func<Task> act = async () => await useCase.Execute(request);

        await act().ShouldNotThrowAsync();

        user.Name.ShouldBe(request.Name);
        user.Email.ShouldBe(request.Email);
    }

    [Fact]
    public async Task Error_NameEmpty()
    {
        (var user, _) = UserBuilder.Build();

        var request = UpdateUserRequestBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreateUseCase(user);

        var ex = await Should.ThrowAsync<ErrorOnValidationException>(async () =>
        {
            await useCase.Execute(request);
        });

        ex.ErrorMessages.Count.ShouldBe(1);
        ex.ErrorMessages.ShouldContain(ResourceMessagesException.NAME_EMPTY);
        user.Name.ShouldNotBe(request.Name);
        user.Email.ShouldNotBe(request.Email);
    }

    [Fact]
    public async Task Error_EmailAlreadyRegistered()
    {
        (var user, _) = UserBuilder.Build();

        var request = UpdateUserRequestBuilder.Build();

        var useCase = CreateUseCase(user, request.Email);

        var ex = await Should.ThrowAsync<ErrorOnValidationException>(async () =>
        {
            await useCase.Execute(request);
        });

        ex.ErrorMessages.Count.ShouldBe(1);
        ex.ErrorMessages.ShouldContain(ResourceMessagesException.EMAIL_ALREADY_REGISTERED);
        user.Name.ShouldNotBe(request.Name);
        user.Email.ShouldNotBe(request.Email);
    }

    private static UpdateUserUseCase CreateUseCase(Cookbook.Domain.Entities.User user, string? email = null)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        var userReadOnlyRepositoryBuilder = new UserReadOnlyRepositoryBuilder().GetById(user);

        if (!string.IsNullOrEmpty(email))
            userReadOnlyRepositoryBuilder.ExistActiveUserWithEmail(email);

        return new UpdateUserUseCase(loggedUser, userReadOnlyRepositoryBuilder.Build(), unitOfWork);
    }
}
