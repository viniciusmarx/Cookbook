using Cookbook.Communication.Requests;
using Cookbook.Communication.Responses;
using Cookbook.Exceptions.ExceptionsBase;

namespace Cookbook.Application.UseCases.User.Register;

public class RegisterUser
{
    public RegisterUserResponse Execute(RegisterUserRequest request)
    {
        Validate(request);

        return new RegisterUserResponse
        {
            Name = request.Name,
        };
    }

    private void Validate(RegisterUserRequest request)
    {
        var validator = new RegisterUserValidator();

        var result = validator.Validate(request);

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage);
            throw new ErrorOnValidationException(errorMessages.ToList());
        }
    }
}
