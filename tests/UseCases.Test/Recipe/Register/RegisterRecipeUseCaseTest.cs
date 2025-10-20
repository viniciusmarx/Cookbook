using CommomTestUtilities.Entities;
using CommomTestUtilities.LoggedUser;
using CommomTestUtilities.Mapper;
using CommomTestUtilities.Repositories;
using CommomTestUtilities.Requests;
using Cookbook.Application.UseCases.Recipe;
using Cookbook.Application.UseCases.Recipe.Register;
using Cookbook.Communication.Requests;
using Cookbook.Exceptions;
using Cookbook.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.Recipe.Register;

public class RegisterRecipeUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, _) = UserBuilder.Build();

        var request = RecipeRequestBuilder.Build();

        var useCase = CreateUseCase(user);

        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.Id.ShouldNotBeNullOrWhiteSpace();
        result.Title.ShouldBe(request.Title);
    }

    [Fact]
    public async Task Error_TitleEmpty()
    {
        (var user, _) = UserBuilder.Build();

        var request = RecipeRequestBuilder.Build();
        request.Title = string.Empty;

        var useCase = CreateUseCase(user);

        var ex = await Should.ThrowAsync<ErrorOnValidationException>(async () =>
        {
            await useCase.Execute(request);
        });

        ex.ErrorMessages.Count.ShouldBe(1);
        ex.ErrorMessages.ShouldContain(ResourceMessagesException.RECIPE_TITLE_EMPTY);
    }

    private static RegisterRecipeUseCase CreateUseCase(Cookbook.Domain.Entities.User user)
    {
        var mapper = MapperBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = new RecipeRepositoryBuilder().Build();

        return new RegisterRecipeUseCase(repository, loggedUser, unitOfWork, mapper);
    }
}
