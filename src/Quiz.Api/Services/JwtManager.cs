using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Quiz.Application.Abstractions;
using Quiz.Application.Common;
using Quiz.Application.Users.Dtos;
using Quiz.Core.Abstractions;
using Quiz.Core.Common;
using Quiz.Core.Entities;
using Quiz.Core.Repositories;

namespace Quiz.Api.Services;

public class JwtManager(IOptions<JwtOptions> options, 
    IRefreshTokenCookieManager cookie, IAuthRepository repo) : IJwtManager
{
    public async Task<OperationResult<User>> GetUserRefreshTokenAsync()
    {
        var refreshToken = cookie.GetRefreshTokenCookie();
        if (!refreshToken.Success)
            return OperationResult<User>.Failure(refreshToken.Errors);
        
        var result = await repo.GetUserAsync(refreshToken.Data!);
        
        return !result.Success
            ? OperationResult<User>.Failure(result.Errors)
            : OperationResult<User>.SuccessResult(result.Data!);
    }
    
    public async Task<IResponse> GenerateTokensAsync(User user)
    {
        var accessToken = GenerateAccessToken(user);
        var refreshToken = await ManageRefreshTokenAsync(user);
        
        return !refreshToken.Success
            ? new ErrorResponse(refreshToken.Message, refreshToken.Errors)
            : new AuthResponse(accessToken, user.MapToUserResponse());
    }
    
    private TokenResponse GenerateAccessToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.JWT_SECRET));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var tokenExpires = DateTime.UtcNow.AddMinutes(options.Value.ACCESS_TOKEN_EXPIRY_MINUTES);
        
        var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Sub, user.Id),
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new (JwtRegisteredClaimNames.Iss, options.Value.JWT_ISSUER),
        };
        
        var token = new JwtSecurityToken(
            issuer: options.Value.JWT_ISSUER,
            audience: options.Value.JWT_AUDIENCE,
            claims: claims,
            expires: tokenExpires,
            signingCredentials: credentials
        );
        
        return new TokenResponse
        {
            ExpiresIn = (int)(tokenExpires - DateTime.UtcNow).TotalSeconds,
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
        };
    }
    
    private async Task<OperationResult> ManageRefreshTokenAsync(User user)
    {
        var expiresIn = options.Value.REFRESH_TOKEN_EXPIRY_DAYS;
        var oldRefreshToken = cookie.GetRefreshTokenCookie();
        var newRefreshToken = GenerateRefreshToken();

        var result = oldRefreshToken.Success
            ? await repo.UpdateRefreshTokenAsync(user, oldRefreshToken.Data!, newRefreshToken, expiresIn)
            : await repo.AddRefreshTokenAsync(user, newRefreshToken, expiresIn);
        
        if (!result.Success)
            return OperationResult.Failure(result.Errors);
        
        cookie.SetRefreshTokenCookie(newRefreshToken, expiresIn, user.RememberMe);
        
        return OperationResult.SuccessResult();
    }
    
    private static string GenerateRefreshToken(int size = 32)
    {
        var randomBytes = new byte[size];
        
        using var rng = RandomNumberGenerator.Create();
        
        rng.GetBytes(randomBytes);
        
        return Convert.ToBase64String(randomBytes);
    }
}