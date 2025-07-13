using CommomTestUtilities.Requests;
using Cookbook.Communication.Responses;
using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace WebApi.Test.User.Register;

public class RegisterUserTest(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient = factory.CreateClient();

    [Fact]
    public async Task Success()
    {
        var request = RegisterUserRequestBuilder.Build();

        var response = await _httpClient.PostAsJsonAsync("User", request);

        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        var responseData = await response.Content.ReadFromJsonAsync<RegisterUserResponse>();

        responseData.ShouldNotBeNull();
        responseData.Name.ShouldBe(request.Name);
    }
}