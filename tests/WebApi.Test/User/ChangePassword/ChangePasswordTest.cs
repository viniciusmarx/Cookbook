using CommomTestUtilities.Requests;
using CommomTestUtilities.Tokens;
using Cookbook.Communication.Requests;
using Cookbook.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.ChangePassword;

public class ChangePasswordTest : CookbookClassFixture
{
    private readonly string _method = "user/change-password";
    private readonly string _password;
    private readonly string _email;
    private readonly Guid _userIdentifier;

    public ChangePasswordTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _password = factory.GetPassword();
        _email = factory.GetEmail();
        _userIdentifier = factory.GetUserIdentifier();
    }

    [Fact]
    public async Task Success()
    {
        var request = ChangePasswordRequestBuilder.Build();
        request.Password = _password;

        var token = JwtTokenGeneratorBuilder.Build().GenerateToken(_userIdentifier);

        var response = await DoPatch(_method, request, token);

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        var loginRequest = new LoginRequest
        {
            Email = _email,
            Password = _password,
        };

        response = await DoPost("login", loginRequest);
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        loginRequest.Password = request.NewPassword;

        response = await DoPost("login", loginRequest);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_NewPasswordEmpty(string culture)
    {
        var request = new ChangePasswordRequest
        {
            Password = _password,
            NewPassword = string.Empty
        };

        var token = JwtTokenGeneratorBuilder.Build().GenerateToken(_userIdentifier);

        var response = await DoPatch(_method, request, token, culture);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var responseData = await response.Content.ReadFromJsonAsync<JsonDocument>();

        responseData.ShouldNotBeNull();

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray().Select(e => e.GetString()).ToList();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("PASSWORD_EMPTY", new CultureInfo(culture));

        errors.ShouldHaveSingleItem();
        errors.ShouldContain(expectedMessage);
    }

    [Fact]
    public async Task Error_TokenInvalid()
    {
        var request = new ChangePasswordRequest();

        var response = await DoPatch(_method, request, token: "tokenInvalid");

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_TokenWithUserNotFound()
    {
        var token = JwtTokenGeneratorBuilder.Build().GenerateToken(Guid.NewGuid());

        var request = new ChangePasswordRequest();

        var response = await DoPatch(_method, request, token);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
