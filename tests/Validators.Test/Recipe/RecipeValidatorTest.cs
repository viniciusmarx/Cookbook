using CommomTestUtilities.Requests;
using Cookbook.Application.UseCases.Recipe;
using Cookbook.Domain.Enums;
using Cookbook.Exceptions;
using Shouldly;

namespace Validators.Test.Recipe;

public class RecipeValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new RecipeValidator();

        var request = RecipeRequestBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void Success_CookingTimeNull()
    {
        var validator = new RecipeValidator();

        var request = RecipeRequestBuilder.Build();
        request.CookingTime = null;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void Success_DifficultyNull()
    {
        var validator = new RecipeValidator();

        var request = RecipeRequestBuilder.Build();
        request.Difficulty = null;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void Error_InvalidCookingTime()
    {
        var validator = new RecipeValidator();

        var request = RecipeRequestBuilder.Build();
        request.CookingTime = (CookingTime)100;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Count.ShouldBe(1);
        result.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.COOKING_TIME_NOT_SUPPORTED));
    }

    [Fact]
    public void Error_InvalidDifficulty()
    {
        var validator = new RecipeValidator();

        var request = RecipeRequestBuilder.Build();
        request.Difficulty = (Difficulty)100;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Count.ShouldBe(1);
        result.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.DIFFICULTY_LEVEL_NOT_SUPPORTED));
    }

    [Theory]
    [InlineData(null), InlineData(""), InlineData("  ")]
    public void Error_InvalidTitle(string title)
    {
        var validator = new RecipeValidator();

        var request = RecipeRequestBuilder.Build();
        request.Title = title;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Count.ShouldBe(1);
        result.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.RECIPE_TITLE_EMPTY));
    }
}
