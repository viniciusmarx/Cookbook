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

    [Fact]
    public void Success_DishTypesEmpty()
    {
        var validator = new RecipeValidator();

        var request = RecipeRequestBuilder.Build();
        request.DishTypes.Clear();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void Error_InvalidDishTypes()
    {
        var validator = new RecipeValidator();

        var request = RecipeRequestBuilder.Build();
        request.DishTypes.Add((DishType)1000);

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Count.ShouldBe(1);
        result.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.DISH_TYPE_NOT_SUPPORTED));
    }

    [Fact]
    public void Error_EmptyIngredients()
    {
        var validator = new RecipeValidator();

        var request = RecipeRequestBuilder.Build();
        request.Ingredients.Clear();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Count.ShouldBe(1);
        result.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.AT_LEAST_ONE_INGREDIENT));
    }

    [Fact]
    public void Error_EmptyInstructions()
    {
        var validator = new RecipeValidator();

        var request = RecipeRequestBuilder.Build();
        request.Instructions.Clear();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Count.ShouldBe(1);
        result.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.AT_LEAST_ONE_INSTRUCTION));
    }

    [Theory]
    [InlineData(null), InlineData(""), InlineData("  ")]
    public void Error_EmptyValueIngredients(string ingredient)
    {
        var validator = new RecipeValidator();

        var request = RecipeRequestBuilder.Build();
        request.Ingredients.Add(ingredient);

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Count.ShouldBe(1);
        result.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.INGREDIENT_EMPTY));
    }

    [Fact]
    public void Error_SameStepInstructions()
    {
        var validator = new RecipeValidator();

        var request = RecipeRequestBuilder.Build();
        request.Instructions.First().Step = request.Instructions.Last().Step;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Count.ShouldBe(1);
        result.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.TWO_OR_MORE_INSTRUCTIONS_SAME_ORDER));
    }

    [Fact]
    public void Error_NegativeStepInstructions()
    {
        var validator = new RecipeValidator();

        var request = RecipeRequestBuilder.Build();
        request.Instructions.First().Step = -1;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Count.ShouldBe(1);
        result.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.NON_NEGATIVE_INSTRUCTION_STEP));
    }

    [Theory]
    [InlineData(null), InlineData(""), InlineData("  ")]
    public void Error_EmptyValueInstructions(string instruction)
    {
        var validator = new RecipeValidator();

        var request = RecipeRequestBuilder.Build();
        request.Instructions.First().Text = instruction;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Count.ShouldBe(1);
        result.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.INSTRUCTION_EMPTY));
    }

    [Fact]
    public void Error_InstructionsTooLong()
    {
        var request = RecipeRequestBuilder.Build();
        request.Instructions.First().Text = new string('a', 2001);

        var validator = new RecipeValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Count.ShouldBe(1);
        result.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.INSTRUCTION_EXCEEDS_LIMIT_CHARACTERS));
    }
}
