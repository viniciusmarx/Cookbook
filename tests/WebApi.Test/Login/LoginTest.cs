using CommomTestUtilities.Requests;
using Cookbook.Communication.Requests;
using Cookbook.Communication.Responses;
using Cookbook.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Login;

public class LoginTest : CookbookClassFixture
{
    private readonly string _method = "login";
    private readonly string _email;
    private readonly string _password;
    private readonly string _name;

    public LoginTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _email = factory.GetEmail();
        _password = factory.GetPassword();
        _name = factory.GetName();
    }

    [Fact]
    public async Task Success()
    {
        var request = new LoginRequest
        {
            Email = _email,
            Password = _password
        };

        var response = await DoPost(_method, request);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var responseData = await response.Content.ReadFromJsonAsync<RegisterUserResponse>();

        responseData.ShouldNotBeNull();
        responseData.Name.ShouldBe(_name);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_LoginInvalid(string culture)
    {
        var request = LoginRequestBuilder.Build();

        var response = await DoPost(_method, request, culture);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        var responseData = await response.Content.ReadFromJsonAsync<JsonDocument>();

        responseData.ShouldNotBeNull();

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray().Select(e => e.GetString()).ToList();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("EMAIL_OR_PASSWORD_INVALID", new CultureInfo(culture));

        errors.ShouldHaveSingleItem();
        errors.ShouldContain(expectedMessage);
    }
}