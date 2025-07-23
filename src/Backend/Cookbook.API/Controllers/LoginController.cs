using Cookbook.Application.UseCases.Login;
using Cookbook.Communication.Requests;
using Cookbook.Communication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Cookbook.API.Controllers;

[Route("[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(RegisterUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromServices] ILoginUseCase useCase, [FromBody] LoginRequest request)
    {
        var response = await useCase.Execute(request);

        return Ok(response);
    }
}