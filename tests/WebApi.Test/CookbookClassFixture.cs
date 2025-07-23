using Azure.Core;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace WebApi.Test;

public class CookbookClassFixture(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient = factory.CreateClient();

    protected async Task<HttpResponseMessage> DoPost(string method, object request, string culture = "en")
    {
        ChangeRequestCulture(culture);

        return await _httpClient.PostAsJsonAsync(method, request);
    }

    private void ChangeRequestCulture(string culture)
    {
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Clear();
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(culture));
    }
}
