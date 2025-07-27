using Cookbook.Domain.Entities;
using Cookbook.Domain.Security.Tokens;
using Cookbook.Domain.Services.LoggedUser;
using Cookbook.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Cookbook.Infrastructure.Services.LoggedUser;

public class LoggedUser(CookbookDbContext dbContext, ITokenProvider tokenProvider) : ILoggedUser
{
    private readonly CookbookDbContext _dbContext = dbContext;
    private readonly ITokenProvider _tokenProvider = tokenProvider;

    public async Task<User> User()
    {
        var token = _tokenProvider.Value();

        var jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

        var userIdentifier = Guid.Parse(jwtSecurityToken.Claims.First(c => c.Type == ClaimTypes.Sid).Value);

        return await _dbContext.Users.AsNoTracking().FirstAsync(user => user.IsActive && user.UserIdentifier == userIdentifier);
    }
}
