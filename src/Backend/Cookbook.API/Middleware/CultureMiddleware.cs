using System.Globalization;

namespace Cookbook.API.Middleware;

public class CultureMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task Invoke(HttpContext context)
    {
        var language = "en-US";
        var supportedLanguages = new List<string> { "pt-BR" };

        var requestedCulture = context.Request.Headers.AcceptLanguage.FirstOrDefault();

        if (!string.IsNullOrWhiteSpace(requestedCulture) && supportedLanguages.Any(c => c.Equals(requestedCulture)))
        {
            language = requestedCulture;
        }

        var cultureInfo = new CultureInfo(language);
        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;

        await _next(context);
    }
}
