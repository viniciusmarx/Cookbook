using Cookbook.Application.UseCases.Recipe.Filter;
using CommomTestUtilities.Entities;
using CommomTestUtilities.LoggedUser;
using CommomTestUtilities.Mapper;
using CommomTestUtilities.Repositories;
using CommomTestUtilities.Requests;
using Shouldly;

namespace UseCases.Test.Recipe.Filter;

using Cookbook.Domain.Entities;
using Cookbook.Domain.Enums;
using Cookbook.Exceptions;
using Cookbook.Exceptions.ExceptionsBase;

public class FilterRecipeUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, _) = UserBuilder.Build();

        var request = FilterRecipeRequestBuilder.Build();

        var recipes = RecipeBuilder.GenerateRecipes(user);

        var useCase = CreateUseCase(user, recipes);

        var response = await useCase.Execute(request);

        response.ShouldNotBeNull();
        response.Count().ShouldBe(recipes.Count);

    }

    [Fact]
    public async Task Error_CookingTimeInvalid()
    {
        (var user, _) = UserBuilder.Build();

        var request = FilterRecipeRequestBuilder.Build();
        request.CookingTimes.Add((Cookbook.Domain.Enums.CookingTime)1000);

        var recipes = RecipeBuilder.GenerateRecipes(user);

        var useCase = CreateUseCase(user, recipes);

        var exception = await Should.ThrowAsync<ErrorOnValidationException>(async () =>
        {
            await useCase.Execute(request);
        });

        exception.ErrorMessages.Count.ShouldBe(1);
        exception.ErrorMessages.ShouldContain(ResourceMessagesException.COOKING_TIME_NOT_SUPPORTED);
    }

    private static FilterRecipeUseCase CreateUseCase(User user, IList<Recipe> recipes)
    {
        var mapper = MapperBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = new RecipeRepositoryBuilder().Filter(user, recipes).Build();

        return new FilterRecipeUseCase(repository, mapper, loggedUser);
    }
}
