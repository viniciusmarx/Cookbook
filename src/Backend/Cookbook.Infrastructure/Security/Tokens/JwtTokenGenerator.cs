﻿using Cookbook.Domain.Security.Tokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Cookbook.Infrastructure.Security.Tokens;

public class JwtTokenGenerator(uint expirationTimeMinutes, string signingKey) : JwtTokenHandler, IAccessTokenGenerator
{
    private readonly uint _expirationTimeMinutes = expirationTimeMinutes;
    private readonly string _signingKey = signingKey;

    public string GenerateToken(Guid userIdentifier)
    {
        var claims = new List<Claim>()
        {
            new(ClaimTypes.Sid, userIdentifier.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_expirationTimeMinutes),
            SigningCredentials = new SigningCredentials(SecurityKey(_signingKey), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
