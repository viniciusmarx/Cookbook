using Cookbook.Application.UseCases.User.Register;
using Cookbook.Communication.Requests;
using Cookbook.Communication.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cookbook.API.Controllers;

[Route("[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(RegisterUserResponse), StatusCodes.Status201Created)]
    public IActionResult Register(RegisterUserRequest request)
    {
        var useCase = new RegisterUser();

        var result = useCase.Execute(request);

        return Created(string.Empty, result);
    }
}
