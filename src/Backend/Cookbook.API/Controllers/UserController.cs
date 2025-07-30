using Cookbook.API.Attributes;
using Cookbook.Application.UseCases.User.ChangePassword;
using Cookbook.Application.UseCases.User.Profile;
using Cookbook.Application.UseCases.User.Register;
using Cookbook.Application.UseCases.User.Update;
using Cookbook.Communication.Requests;
using Cookbook.Communication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Cookbook.API.Controllers;

[Route("[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(RegisterUserResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request, [FromServices] IRegisterUser registerUser)
    {
        var result = await registerUser.Execute(request);

        return Created(string.Empty, result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(UserProfileResponse), StatusCodes.Status200OK)]
    [AuthenticatedUser]
    public async Task<IActionResult> GetUserProfile([FromServices] IGetUserProfileUseCase useCase)
    {
        var result = await useCase.Execute();

        return Ok(result);
    }

    [HttpPatch]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [AuthenticatedUser]
    public async Task<IActionResult> Update([FromServices] IUpdateUserUseCase useCase, [FromBody] UpdateUserRequest request)
    {
        await useCase.Execute(request);

        return NoContent();
    }

    [HttpPatch("change-password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [AuthenticatedUser]
    public async Task<IActionResult> ChangePassword([FromServices] IChangePasswordUseCase useCase, [FromBody] ChangePasswordRequest request)
    {
        await useCase.Execute(request);

        return NoContent();
    }
}