using Cookbook.Application.SharedValidators;
using Cookbook.Communication.Requests;
using Cookbook.Exceptions;
using FluentValidation;

namespace Cookbook.Application.UseCases.User.Register;

public class RegisterUserValidator : AbstractValidator<RegisterUserRequest>
{
    public RegisterUserValidator()
    {
        RuleFor(user => user.Name).NotEmpty().WithMessage(ResourceMessagesException.NAME_EMPTY);

        RuleFor(user => user.Email).NotEmpty().WithMessage(ResourceMessagesException.EMAIL_EMPTY);

        RuleFor(user => user.Password).SetValidator(new PasswordValidator<RegisterUserRequest>());

        When(user => !string.IsNullOrEmpty(user.Email), () =>
        {
            RuleFor(user => user.Email).EmailAddress().WithMessage(ResourceMessagesException.EMAIL_INVALID);
        });
    }
}