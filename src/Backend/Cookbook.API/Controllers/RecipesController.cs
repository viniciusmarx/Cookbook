using Azure;
using Cookbook.API.Attributes;
using Cookbook.Application.UseCases.Recipe.Filter;
using Cookbook.Application.UseCases.Recipe.Register;
using Cookbook.Communication.Requests;
using Cookbook.Communication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Cookbook.API.Controllers;

[ApiController]
[Route("[controller]")]
[AuthenticatedUser]
public class RecipesController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(RegisteredRecipeResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RecipeRequest request, [FromServices] IRegisterRecipeUseCase registerRecipe)
    {
        var response = await registerRecipe.Execute(request);

        return Created(string.Empty, response);
    }

    [HttpPost("filter")]
    [ProducesResponseType(typeof(RecipesResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Filter([FromBody] FilterRecipeRequest request, [FromServices] IFilterRecipeUseCase useCase)
    {
        var response = await useCase.Execute(request);

        if (response.Any())
            return Ok(response);

        return NoContent();
    }
}
