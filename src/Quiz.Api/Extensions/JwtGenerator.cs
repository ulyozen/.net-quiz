using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Quiz.Application.Abstractions;
using Quiz.Application.Common;
using Quiz.Core.Entities;
using Quiz.Persistence.Common;

namespace Quiz.Api.Extensions;

public class JwtGenerator(IOptions<JwtOptions> options) : IJwtGenerator
{
    public TokenResponse GenerateToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.Secret!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var tokenExpires = DateTime.UtcNow.AddMinutes(double.Parse(options.Value.AccessTokenExpiryMinutes!));
        
        var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Sub, user.Id!),
            new (JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUniversalTime().ToString()),
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new ("role", user.Role!)
        };
        
        var token = new JwtSecurityToken(
            issuer: options.Value.Issuer,
            audience: options.Value.Audience,
            claims: claims,
            expires: tokenExpires,
            signingCredentials: credentials
        );
        
        return new TokenResponse
        {
            ExpiresIn = (int)(tokenExpires - DateTime.UtcNow).TotalSeconds,
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = GenerateRefreshToken(),
        };
    }
    
    public string UpdateRefreshToken(string refreshToken)
    {
        return GenerateRefreshToken();
    }
    
    private static string GenerateRefreshToken(int size = 32)
    {
        var randomBytes = new byte[size];
        
        using var rng = RandomNumberGenerator.Create();
        
        rng.GetBytes(randomBytes);
        
        return Convert.ToBase64String(randomBytes);
    }
}