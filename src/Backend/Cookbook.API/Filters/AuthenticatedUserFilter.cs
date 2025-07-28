using Cookbook.Communication.Responses;
using Cookbook.Domain.Repositories.User;
using Cookbook.Domain.Security.Tokens;
using Cookbook.Exceptions;
using Cookbook.Exceptions.ExceptionsBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;

namespace Cookbook.API.Filters;

public class AuthenticatedUserFilter(IAccessTokenValidator accessTokenValidator, IUserReadOnlyRepository repository) : IAsyncAuthorizationFilter
{
    private readonly IAccessTokenValidator _accessTokenValidator = accessTokenValidator;
    private readonly IUserReadOnlyRepository _repository = repository;

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        try
        {
            var token = TokenOnRequest(context);

            var userIdentifier = _accessTokenValidator.ValidateAndGetUserIdentifier(token);

            var exist = await _repository.ExistActiveUserWithIdentifier(userIdentifier);
            if (!exist)
            {
                throw new CookbookException(ResourceMessagesException.USER_WITHOUT_PERMISSION_ACCESS_RESOURCE);
            }
        }
        catch (SecurityTokenExpiredException)
        {
            context.Result = new UnauthorizedObjectResult(new ErrorResponse("TokenIsExpired")
            {
                TokenIsExpired = true
            });
        }
        catch (CookbookException ex)
        {
            context.Result = new UnauthorizedObjectResult(new ErrorResponse(ex.Message));
        }
        catch
        {
            context.Result = new UnauthorizedObjectResult(new ErrorResponse(ResourceMessagesException.USER_WITHOUT_PERMISSION_ACCESS_RESOURCE));
        }
    }

    private static string TokenOnRequest(AuthorizationFilterContext context)
    {
        var authentication = context.HttpContext.Request.Headers.Authorization.ToString();

        if (string.IsNullOrEmpty(authentication))
            throw new CookbookException(ResourceMessagesException.NO_TOKEN);

        return authentication["Bearer ".Length..].Trim();
    }
}