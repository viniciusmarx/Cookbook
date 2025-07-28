using CommomTestUtilities.Tokens;
using Cookbook.Communication.Responses;
using Shouldly;
using System.Net.Http.Json;
using System.Text.Json;

namespace WebApi.Test.User.Profile;

public class GetUserProfileTest : CookbookClassFixture
{
    private readonly string _method = "user";
    private readonly string _name;
    private readonly string _email;
    private readonly Guid _userIdentifier;

    public GetUserProfileTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _name = factory.GetName();
        _email = factory.GetEmail();
        _userIdentifier = factory.GetUserIdentifier();
    }

    [Fact]
    public async Task Success()
    {
        var token = JwtTokenGeneratorBuilder.Build().GenerateToken(_userIdentifier);

        var response = await DoGet(_method, token);

        var responseData = await response.Content.ReadFromJsonAsync<UserProfileResponse>();

        responseData.ShouldNotBeNull();
        responseData.ShouldSatisfyAllConditions(data =>
        {
            data.Name.ShouldNotBeNullOrWhiteSpace();
            data.Name.ShouldBe(_name);
            data.Email.ShouldNotBeNullOrWhiteSpace();
            data.Email.ShouldBe(_email);
        });
    }
}