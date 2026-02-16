using CommomTestUtilities.Entities;
using CommomTestUtilities.LoggedUser;
using CommomTestUtilities.Mapper;
using CommomTestUtilities.Repositories;
using Cookbook.Application.UseCases.Recipe.GetById;
using Cookbook.Exceptions;
using Cookbook.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.Recipe.GetById;

public class GetRecipeByIdUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, _) = UserBuilder.Build();
        var recipe = RecipeBuilder.GenerateRecipes(user, 1);

        var useCase = CreateUseCase(user, recipe[0]);

        var result = await useCase.Execute(recipe[0].Id);

        result.ShouldNotBeNull();
        result.Id.ShouldNotBeNullOrWhiteSpace();
        result.Title.ShouldBe(recipe[0].Title);
    }

    [Fact]
    public async Task Error_RecipeNotFound()
    {
        (var user, _) = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        var exception = await Should.ThrowAsync<NotFoundException>(() => useCase.Execute(recipeId: 1));

        exception.Message.ShouldBe(ResourceMessagesException.RECIPE_NOT_FOUND);
    }

    private static GetRecipeByIdUseCase CreateUseCase(Cookbook.Domain.Entities.User user, Cookbook.Domain.Entities.Recipe? recipe = null)
    {
        var mapper = MapperBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = new RecipeRepositoryBuilder().GetById(user, recipe).Build();

        return new GetRecipeByIdUseCase(mapper, loggedUser, repository);
    }
}
