using CommomTestUtilities.Requests;
using Cookbook.Application.UseCases.User.ChangePassword;
using Cookbook.Communication.Requests;
using Cookbook.Exceptions;
using Shouldly;

namespace Validators.Test.User.ChangePassword;

public class ChangePasswordValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new ChangePasswordValidator();

        var request = ChangePasswordRequestBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData(0), InlineData(1), InlineData(2), InlineData(3), InlineData(4), InlineData(5)]
    public void Error_PasswordInvalid(int passwordLength)
    {
        var validator = new ChangePasswordValidator();

        var request = ChangePasswordRequestBuilder.Build(passwordLength);

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Count.ShouldBe(1);
        result.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.PASSWORD_EMPTY));
    }
}