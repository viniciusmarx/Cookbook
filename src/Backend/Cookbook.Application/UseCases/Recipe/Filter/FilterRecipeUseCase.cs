using AutoMapper;
using Cookbook.Communication.Requests;
using Cookbook.Communication.Responses;
using Cookbook.Domain.Enums;
using Cookbook.Domain.Repositories.Recipe;
using Cookbook.Domain.Services.LoggedUser;
using Cookbook.Domain.ValueObjects;
using Cookbook.Exceptions.ExceptionsBase;

namespace Cookbook.Application.UseCases.Recipe.Filter;

public class FilterRecipeUseCase(IRecipeRepository repository, IMapper mapper, ILoggedUser loggedUser) : IFilterRecipeUseCase
{
    private readonly IRecipeRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly ILoggedUser _loggedUser = loggedUser;

    public async Task<IEnumerable<RecipesResponse>> Execute(FilterRecipeRequest request)
    {
        Validate(request);

        var loggedUser = await _loggedUser.User();

        var filters = new RecipeFilters(
            request.IngredientTitle,
            request.CookingTimes.Distinct().Select(c => (CookingTime)c).ToList(),
            request.Difficulties.Distinct().Select(c => (Difficulty)c).ToList(),
            request.DishTypes);

        var recipes = await _repository.Filter(loggedUser, filters);

        return _mapper.Map<IEnumerable<RecipesResponse>>(recipes);
    }

    private static void Validate(FilterRecipeRequest request)
    {
        var validator = new FilterRecipeValidator();

        var result = validator.Validate(request);

        if (!result.IsValid)
        {
            var errors = result.Errors.Select(e => e.ErrorMessage).Distinct().ToList();

            throw new ErrorOnValidationException(errors);
        }
    }
}
