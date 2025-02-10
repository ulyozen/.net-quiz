using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Quiz.Application.Abstractions;
using Quiz.Application.Common;
using Quiz.Core.Abstractions;
using Quiz.Core.Common;
using Quiz.Core.Entities;

namespace Quiz.Api.Extensions;

public class JwtManager(
    IOptions<JwtOptions> options, 
    IRefreshTokenCookieManager cookie, 
    IUserRepository userRepo) 
    : IJwtManager
{
    public async Task<OperationResult<User>> GetRefreshTokenAsync()
    {
        var refreshToken = cookie.GetRefreshTokenCookie();
        if (!refreshToken.Success)
            return OperationResult<User>.Failure(refreshToken.Errors!);
        
        var result = await userRepo.GetUserByRefreshTokenAsync(refreshToken.Data!);
        
        return !result.Success
            ? OperationResult<User>.Failure(result.Errors!)
            : OperationResult<User>.SuccessResult(result.Data!);
    }
    
    public async Task<AuthResponse> GenerateTokensAsync(User user)
    {
        var accessToken = GenerateAccessToken(user);
        var refreshToken = await ManageRefreshTokenAsync(user.Id!);
        
        return !refreshToken.Success
            ? new AuthResponse { Success = false, Errors = refreshToken.Errors }
            : new AuthResponse
            {
                Success = true,
                Token = accessToken,
                User = user.MapToUserInfo()
            };
    }
    
    private async Task<OperationResult> ManageRefreshTokenAsync(string userId)
    {
        var expiresIn = options.Value.RefreshTokenExpiryDays!;
        var newRefreshToken = GenerateRefreshToken();
        var oldRefreshToken = cookie.GetRefreshTokenCookie();
        
        var result = oldRefreshToken.Success
            ? await userRepo.UpdateRefreshTokenAsync(oldRefreshToken.Data!, newRefreshToken, expiresIn)
            : await userRepo.AddRefreshTokenAsync(userId, newRefreshToken, expiresIn);
        
        if (!result.Success)
            return OperationResult.Failure(result.Errors!);
        
        cookie.SetRefreshTokenCookie(newRefreshToken, expiresIn);
        
        return OperationResult.SuccessResult();
    }
    
    private TokenResponse GenerateAccessToken(User user)
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
        };
    }
    
    private static string GenerateRefreshToken(int size = 32)
    {
        var randomBytes = new byte[size];
        
        using var rng = RandomNumberGenerator.Create();
        
        rng.GetBytes(randomBytes);
        
        return Convert.ToBase64String(randomBytes);
    }
}