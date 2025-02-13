using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quiz.Application.Abstractions;
using Quiz.Core.Abstractions;
using Quiz.Core.Common;
using Quiz.Core.Entities;
using Quiz.Persistence.Common;
using Quiz.Persistence.Context;
using Quiz.Persistence.Entities;

namespace Quiz.Persistence.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly AppDbContext _context;
    private readonly UserManager<UserEntity> _userManager;
    private readonly SignInManager<UserEntity> _signInManager;
    private readonly IRefreshTokenCookieManager _cookie;
    
    public AuthRepository(
        AppDbContext context, 
        UserManager<UserEntity> userManager, 
        SignInManager<UserEntity> signInManager, 
        IRefreshTokenCookieManager cookie)
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
        _cookie = cookie;
    }
    
    public async Task<OperationResult<User>> GetUserAsync(string refreshToken)
    {
        var token = await _context.RefreshTokens
            .Include(rt => rt.UserEntity)
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

        if (token == null)
            return OperationResult<User>.Failure(DomainErrors.Auth.RefreshTokenNotFound);
        
        var role = await _userManager.GetRolesAsync(token.UserEntity);
        var user = token.UserEntity.MapToUser();
        user.Role = string.Join(',', role);

        if (token.Expires < DateTime.UtcNow)
        {
            _context.RefreshTokens.Remove(token);
            await _context.SaveChangesAsync();
            return OperationResult<User>.Failure(DomainErrors.Auth.RefreshTokenExpired);
        }
        
        return OperationResult<User>.SuccessResult(user);
    }
    
    public async Task<OperationResult<User>> AddUserAsync(User user)
    {
        var emailExist = await _userManager.FindByEmailAsync(user.Email!);
        if (emailExist != null) 
            return OperationResult<User>.Failure(DomainErrors.Auth.EmailAlreadyExists);
        
        var userEntity = new UserEntity { Name = user.Username, UserName = user.Email, Email = user.Email };
        
        var result = await _userManager.CreateAsync(userEntity, user.Password!);
        if (!result.Succeeded)
            return OperationResult<User>.Failure(result.Errors.Select(e => e.Description).ToList());
        
        var roleResult = await _userManager.AddToRoleAsync(userEntity, user.Role!);
        if (!roleResult.Succeeded)
        {
            await _userManager.DeleteAsync(userEntity);
            return OperationResult<User>.Failure(roleResult.Errors.Select(e => e.Description).ToList());
        }
        
        return OperationResult<User>.SuccessResult(userEntity.MapToUser());
    }
    
    public async Task<OperationResult<User>> LoginAsync(string email, string password, bool rememberMe)
    {
        var emailExist = await _userManager.FindByEmailAsync(email);
        if (emailExist == null)
            return OperationResult<User>.Failure(DomainErrors.Auth.EmailNotFound);
        
        var checkCredential = await _signInManager.CheckPasswordSignInAsync(emailExist, password, false);
        if (checkCredential.IsLockedOut)
            return OperationResult<User>.Failure(DomainErrors.User.UserBlocked);
        if (!checkCredential.Succeeded)
            return OperationResult<User>.Failure(DomainErrors.Auth.InvalidPassword);
        
        emailExist.RememberMe = rememberMe;
        await _userManager.UpdateAsync(emailExist);
        
        var role = await _userManager.GetRolesAsync(emailExist);
        var user = emailExist.MapToUser();
        user.Role = string.Join(',', role);
        
        return OperationResult<User>.SuccessResult(user);
    }
    
    public async Task<OperationResult> AddRefreshTokenAsync(User user, string refreshToken, string expiryDate)
    {
        var refreshTokenEntity = new RefreshTokenEntity
        {
            Id = Guid.NewGuid().ToString(),
            Token = refreshToken, 
            Expires = user.RememberMe 
                ? DateTime.UtcNow.AddDays(int.Parse(expiryDate))
                : DateTime.UtcNow.AddHours(int.Parse(expiryDate)),
            IsUsed = false,
            IsRevoked = false,
            UserId = user.Id!
        };
        
        await _context.RefreshTokens.AddAsync(refreshTokenEntity);
        await _context.SaveChangesAsync();
        
        return OperationResult.SuccessResult();
    }
    
    public async Task<OperationResult> UpdateRefreshTokenAsync(User user, string oldRefreshToken, string newRefreshToken, string expiryDate)
    {
        var tokenExist = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == oldRefreshToken);
        if (tokenExist == null)
            return OperationResult.Failure(DomainErrors.Auth.RefreshTokenNotFound);
        
        tokenExist.Token = newRefreshToken;
        tokenExist.Expires = user.RememberMe
            ? DateTime.UtcNow.AddDays(int.Parse(expiryDate))
            : DateTime.UtcNow.AddHours(int.Parse(expiryDate));
        await _context.SaveChangesAsync();
        
        return OperationResult.SuccessResult();
    }
    
    public async Task<OperationResult> RecoverPasswordAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return OperationResult.Failure(DomainErrors.Auth.EmailNotFound);
        
        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, resetToken, password);
        
        return !result.Succeeded 
            ? OperationResult.Failure(result.Errors.Select(e => e.Description).ToList())
            : OperationResult.SuccessResult();
    }
    
    public async Task<OperationResult> RevokeAccessAsync()
    {
        var refreshToken = _cookie.GetRefreshTokenCookie();
        if (!refreshToken.Success)
            return OperationResult.Failure(refreshToken.Errors!);
        
        var result = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken.Data);
        if (result == null)
            return OperationResult.Failure(DomainErrors.Auth.RefreshTokenNotFound);
        
        _cookie.RemoveRefreshTokenCookie();
        
        _context.RefreshTokens.Remove(result);
        await _context.SaveChangesAsync();
        
        return OperationResult.SuccessResult();
    }
}