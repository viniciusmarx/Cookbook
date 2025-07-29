using CommomTestUtilities.Requests;
using Cookbook.Application.UseCases.User.Update;
using Cookbook.Communication.Requests;
using Cookbook.Exceptions;
using Shouldly;

namespace Validators.Test.User.Update;

public class UpdateUserValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new UpdateUserValidator();

        var request = UpdateUserRequestBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void Error_NameEmpty()
    {
        var validator = new UpdateUserValidator();

        var request = UpdateUserRequestBuilder.Build();
        request.Name = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Count.ShouldBe(1);
        result.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.NAME_EMPTY));
    }

    [Fact]
    public void Error_EmailEmpty()
    {
        var validator = new UpdateUserValidator();

        var request = UpdateUserRequestBuilder.Build();
        request.Email = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();

        result.Errors.Count.ShouldBe(1);
        result.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.EMAIL_EMPTY));
    }

    [Fact]
    public void Error_EmailInvalid()
    {
        var validator = new UpdateUserValidator();

        var request = UpdateUserRequestBuilder.Build();
        request.Email = "email.com";

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();

        result.Errors.Count.ShouldBe(1);
        result.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.EMAIL_INVALID));
    }
}