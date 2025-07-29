using CommomTestUtilities.Requests;
using CommomTestUtilities.Tokens;
using Cookbook.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Update;

public class UpdateUserTest : CookbookClassFixture
{
    private readonly string _method = "user";

    private readonly Guid _userIdentifier;

    public UpdateUserTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _userIdentifier = factory.GetUserIdentifier();
    }

    [Fact]
    public async Task Success()
    {
        var request = UpdateUserRequestBuilder.Build();

        var token = JwtTokenGeneratorBuilder.Build().GenerateToken(_userIdentifier);

        var response = await DoPatch(_method, request, token);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.NoContent);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_EmptyName(string culture)
    {
        var request = UpdateUserRequestBuilder.Build();
        request.Name = string.Empty;

        var token = JwtTokenGeneratorBuilder.Build().GenerateToken(_userIdentifier);

        var response = await DoPatch(_method, request, token, culture);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var responseData = await response.Content.ReadFromJsonAsync<JsonDocument>();

        responseData.ShouldNotBeNull();

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray().Select(e => e.GetString()).ToList();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(culture));

        errors.ShouldHaveSingleItem();
        errors.ShouldContain(expectedMessage);
    }
}
