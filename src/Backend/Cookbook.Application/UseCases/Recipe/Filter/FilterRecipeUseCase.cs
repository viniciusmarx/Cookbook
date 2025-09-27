using AutoMapper;
using Cookbook.Communication.Requests;
using Cookbook.Communication.Responses;
using Cookbook.Domain.Services.LoggedUser;
using Cookbook.Exceptions.ExceptionsBase;

namespace Cookbook.Application.UseCases.Recipe.Filter;

public class FilterRecipeUseCase(IMapper mapper, ILoggedUser loggedUser) : IFilterRecipeUseCase
{
    private readonly IMapper _mapper = mapper;
    private readonly ILoggedUser _loggedUser = loggedUser;

    public async Task<IEnumerable<RecipesResponse>> Execute(FilterRecipeRequest request)
    {
        Validate(request);

        var loggedUser = await _loggedUser.User();

        var response = new List<RecipesResponse>();

        return response;
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
