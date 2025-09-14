using AutoMapper;
using Cookbook.Communication.Requests;
using Cookbook.Communication.Responses;
using Cookbook.Domain.Entities;
using Cookbook.Domain.Repositories;
using Cookbook.Domain.Repositories.Recipe;
using Cookbook.Domain.Services.LoggedUser;
using Cookbook.Exceptions.ExceptionsBase;

namespace Cookbook.Application.UseCases.Recipe.Register;

public class RegisterRecipeUseCase(IRecipeRepository repository, ILoggedUser loggedUser, IUnitOfWork unitOfWork, IMapper mapper) : IRegisterRecipeUseCase
{
    private readonly IRecipeRepository _repository = repository;
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<RegisteredRecipeResponse> Execute(RecipeRequest request)
    {
        Validate(request);

        var loggedUser = await _loggedUser.User();

        var recipe = _mapper.Map<Domain.Entities.Recipe>(request);
        recipe.UserId = loggedUser.Id;

        var instructions = request.Instructions.OrderBy(i => i.Step).ToList();
        for (var i = 0; i < instructions.Count; i++)
            instructions.ElementAt(i).Step = i + 1;

        recipe.Instructions = _mapper.Map<IList<Domain.Entities.Instruction>>(instructions);

        await _repository.Add(recipe);

        await _unitOfWork.Commit();

        return _mapper.Map<RegisteredRecipeResponse>(recipe);
    }

    private static void Validate(RecipeRequest request)
    {
        var result = new RecipeValidator().Validate(request);

        if (!result.IsValid)
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).Distinct().ToList());
    }
}
