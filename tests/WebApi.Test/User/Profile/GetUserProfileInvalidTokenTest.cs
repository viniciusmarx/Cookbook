using CommomTestUtilities.Tokens;
using Shouldly;
using System.Net;

namespace WebApi.Test.User.Profile;

public class GetUserProfileInvalidTokenTest : CookbookClassFixture
{
    private readonly string _method = "user";

    public GetUserProfileInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory) { }

    [Fact]
    public async Task Error_TokenInvalid()
    {
        var response = await DoGet(_method, token: "tokenInvalid");

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_WithoutToken()
    {
        var response = await DoGet(_method, token: string.Empty);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_TokenWithUserNotFound()
    {
        var token = JwtTokenGeneratorBuilder.Build().GenerateToken(Guid.NewGuid());

        var response = await DoGet(_method, token);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}