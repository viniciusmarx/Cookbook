using CommomTestUtilities.Requests;
using Cookbook.Communication.Responses;
using Cookbook.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Register;

public class RegisterUserTest : CookbookClassFixture
{
    private readonly string _method = "user";

    public RegisterUserTest(CustomWebApplicationFactory factory) : base(factory) { }

    [Fact]
    public async Task Success()
    {
        var request = RegisterUserRequestBuilder.Build();

        var response = await DoPost(_method, request);

        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        var responseData = await response.Content.ReadFromJsonAsync<RegisterUserResponse>();

        responseData.ShouldNotBeNull();
        responseData.Name.ShouldBe(request.Name);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_EmptyName(string culture)
    {
        var request = RegisterUserRequestBuilder.Build();
        request.Name = string.Empty;

        var response = await DoPost(_method, request, culture);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var responseData = await response.Content.ReadFromJsonAsync<JsonDocument>();

        responseData.ShouldNotBeNull();

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray().Select(e => e.GetString()).ToList();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(culture));

        errors.ShouldHaveSingleItem();
        errors.ShouldContain(expectedMessage);
    }
}