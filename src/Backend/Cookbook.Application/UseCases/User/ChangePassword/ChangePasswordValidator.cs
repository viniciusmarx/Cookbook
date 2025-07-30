using Cookbook.Application.SharedValidators;
using Cookbook.Communication.Requests;
using FluentValidation;

namespace Cookbook.Application.UseCases.User.ChangePassword;

public class ChangePasswordValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordValidator()
    {
        RuleFor(req => req.NewPassword).SetValidator(new PasswordValidator<ChangePasswordRequest>());

    }
}