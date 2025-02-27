using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quiz.Application.Abstractions;
using Quiz.Core.Common;
using Quiz.Core.Entities;
using Quiz.Core.Repositories;
using Quiz.Persistence.Common;
using Quiz.Persistence.Context;
using Quiz.Persistence.Entities;
using Quiz.Persistence.Mappers;

namespace Quiz.Persistence.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly AppDbContext _context;
    private readonly IGuidFactory _guidFactory;
    private readonly UserManager<UserEntity> _userManager;
    private readonly SignInManager<UserEntity> _signInManager;
    
    public AuthRepository(
        AppDbContext context,
        IGuidFactory guidFactory,
        UserManager<UserEntity> userManager, 
        SignInManager<UserEntity> signInManager)
    {
        _context = context;
        _guidFactory = guidFactory;
        _userManager = userManager;
        _signInManager = signInManager;
    }
    
    public async Task<OperationResult<User>> GetUserAsync(string refreshToken)
    {
        var token = await _context.RefreshTokens
            .Include(rt => rt.UserEntity)
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken);
        
        if (token is null)
            return OperationResult<User>.Failure(DomainErrors.Auth.RefreshTokenNotFound);
        
        var role = await _userManager.GetRolesAsync(token.UserEntity);
        var user = token.UserEntity.MapToUser();
        user.ChangeRole(string.Join(',', role));
        
        if (token.Expires >= DateTime.UtcNow)
            return OperationResult<User>.SuccessResult(user);
        
        _context.RefreshTokens.Remove(token);
        await _context.SaveChangesAsync();
        
        return OperationResult<User>.Failure(DomainErrors.Auth.RefreshTokenExpired);
    }
    
    public async Task<OperationResult<User>> AddUserAsync(User user)
    {
        var emailExist = await _userManager.FindByEmailAsync(user.Email.Value);
        if (emailExist != null) 
            return OperationResult<User>.Failure(DomainErrors.Auth.EmailAlreadyExists);
        
        var userEntity = user.MapToEntity();
        
        var result = await _userManager.CreateAsync(userEntity, user.Password);
        if (!result.Succeeded)
            return OperationResult<User>.Failure(result.Errors.Select(e => e.Description).ToList());
        
        var roleResult = await _userManager.AddToRoleAsync(userEntity, user.Role);
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
        
        if (emailExist is null)
            return OperationResult<User>.Failure(DomainErrors.Auth.EmailNotFound);
        if (emailExist.IsBlocked)
            return OperationResult<User>.Failure(DomainErrors.User.UserBlocked);
        
        var checkCredential = await _signInManager.CheckPasswordSignInAsync(emailExist, password, false);
        if (!checkCredential.Succeeded)
            return OperationResult<User>.Failure(DomainErrors.Auth.InvalidPassword);
        
        emailExist.RememberMe = rememberMe;
        await _userManager.UpdateAsync(emailExist);
        
        var role = await _userManager.GetRolesAsync(emailExist);
        var user = emailExist.MapToUser();
        user.ChangeRole(string.Join(',', role));
        
        return OperationResult<User>.SuccessResult(user);
    }
    
    public async Task<OperationResult> AddRefreshTokenAsync(User user, string refreshToken, int tokenLifetime)
    {
        var refreshTokenEntity = user.CreateRefreshToken(_guidFactory.Create(), refreshToken, tokenLifetime);
        
        await _context.RefreshTokens.AddAsync(refreshTokenEntity);
        
        await _context.SaveChangesAsync();
        
        return OperationResult.SuccessResult();
    }
    
    public async Task<OperationResult> UpdateRefreshTokenAsync(User user, string oldRefreshToken, 
        string newRefreshToken, int tokenLifetime)
    {
        var existingToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == oldRefreshToken);
        
        if (existingToken is null)
            return OperationResult.Failure(DomainErrors.Auth.RefreshTokenNotFound);
        
        existingToken.UpdateRefreshToken(user, newRefreshToken, tokenLifetime);
        
        _context.RefreshTokens.Update(existingToken);
        
        await _context.SaveChangesAsync();
        
        return OperationResult.SuccessResult();
    }
    
    public async Task<OperationResult> RecoverPasswordAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
            return OperationResult.Failure(DomainErrors.Auth.EmailNotFound);
        
        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        
        var result = await _userManager.ResetPasswordAsync(user, resetToken, password);
        
        return !result.Succeeded 
            ? OperationResult.Failure(result.Errors.Select(e => e.Description).ToList())
            : OperationResult.SuccessResult();
    }
    
    public async Task<OperationResult> RevokeRefreshTokenAsync(string refreshToken)
    {
        var result = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken);
        
        if (result == null)
            return OperationResult.Failure(DomainErrors.Auth.RefreshTokenNotFound);
        
        result.IsRevoked = true;
        
        await _context.SaveChangesAsync();
        
        return OperationResult.SuccessResult();
    }
}