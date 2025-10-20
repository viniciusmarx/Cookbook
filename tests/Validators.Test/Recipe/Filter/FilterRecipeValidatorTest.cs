using CommomTestUtilities.Requests;
using Cookbook.Application.UseCases.Recipe.Filter;
using Shouldly;
using Cookbook.Exceptions;

namespace Validators.Test.Recipe.Filter;

using Cookbook.Domain.Enums;


public class FilterRecipeValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new FilterRecipeValidator();

        var request = FilterRecipeRequestBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void Error_CookingTimeOutOfRange()
    {
        var validator = new FilterRecipeValidator();

        var request = FilterRecipeRequestBuilder.Build();
        request.CookingTimes.Add((CookingTime)1000);

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Count.ShouldBe(1);
        result.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.COOKING_TIME_NOT_SUPPORTED));
    }

    [Fact]
    public void Error_DifficultyOutOfRange()
    {
        var validator = new FilterRecipeValidator();

        var request = FilterRecipeRequestBuilder.Build();
        request.Difficulties.Add((Difficulty)1000);

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Count.ShouldBe(1);
        result.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.DIFFICULTY_LEVEL_NOT_SUPPORTED));
    }

    [Fact]
    public void Error_DishTypeOutOfRange()
    {
        var validator = new FilterRecipeValidator();

        var request = FilterRecipeRequestBuilder.Build();
        request.DishTypes.Add((DishType)1000);

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Count.ShouldBe(1);
        result.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.DISH_TYPE_NOT_SUPPORTED));
    }
}
