using CommomTestUtilities.Requests;
using CommomTestUtilities.Tokens;
using Cookbook.Communication.Responses;
using Cookbook.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Recipe.Register;

public class RegisterRecipeTest : CookbookClassFixture
{
    private const string _method = "recipes";
    private readonly Guid _userIdentifier;

    public RegisterRecipeTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _userIdentifier = factory.GetUserIdentifier();
    }

    [Fact]
    public async Task Success()
    {
        var request = RecipeRequestBuilder.Build();

        var token = JwtTokenGeneratorBuilder.Build().GenerateToken(_userIdentifier);

        var response = await DoPost(method: _method, request: request, token: token);

        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        var responseData = await response.Content.ReadFromJsonAsync<RegisteredRecipeResponse>();

        responseData.ShouldNotBeNull();
        responseData.Title.ShouldBe(responseData.Title);
        responseData.Id.ShouldNotBeNullOrWhiteSpace();
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_TitleEmpty(string culture)
    {
        var request = RecipeRequestBuilder.Build();
        request.Title = string.Empty;

        var token = JwtTokenGeneratorBuilder.Build().GenerateToken(_userIdentifier);

        var response = await DoPost(method: _method, request: request, token: token, culture: culture);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var responseData = await response.Content.ReadFromJsonAsync<JsonDocument>();

        responseData.ShouldNotBeNull();

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray().Select(e => e.GetString()).ToList();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("RECIPE_TITLE_EMPTY", new CultureInfo(culture));

        errors.ShouldHaveSingleItem();
        errors.ShouldContain(expectedMessage);
    }
}
