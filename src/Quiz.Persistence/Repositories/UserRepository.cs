using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quiz.Core.Abstractions;
using Quiz.Core.Common;
using Quiz.Core.Entities;
using Quiz.Persistence.Common;
using Quiz.Persistence.Context;
using Quiz.Persistence.Entities;

namespace Quiz.Persistence.Repositories;

public class UserRepository(
    AppDbContext context,
    UserManager<UserEntity> userManager, 
    SignInManager<UserEntity> signInManager) : IUserRepository
{
    public async Task<OperationResult<User>> GetUserByIdAsync(string userId)
    {
        var emailExist = await userManager.FindByIdAsync(userId);
        
        return emailExist is null 
            ? OperationResult<User>.Failure([$"User with ID {userId} does not exist."]) 
            : OperationResult<User>.SuccessResult(emailExist.MapToUser());
    }
    
    public async Task<OperationResult<User>> GetUserByEmailAsync(string email)
    {
        var emailExist = await userManager.FindByEmailAsync(email!);
        
        return emailExist is null 
            ? OperationResult<User>.Failure([$"Email {email} does not exist."]) 
            : OperationResult<User>.SuccessResult(emailExist.MapToUser());
    }
    
    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        var users = await userManager.Users.ToListAsync();
        
        return users.Select(u => u.MapToUser());
    }
    
    public async Task<OperationResult<User>> GetUserByRefreshTokenAsync(string refreshToken)
    {
        var token = await context.RefreshTokens
            .Include(rt => rt.UserEntity)
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

        if (token == null)
            return OperationResult<User>.Failure(["Refresh token not found"]);
        
        var role = await userManager.GetRolesAsync(token.UserEntity);
        var user = token.UserEntity.MapToUser();
        user.Role = string.Join(',', role);
        
        return token.Expires < DateTime.UtcNow 
            ? OperationResult<User>.Failure(["Refresh token has expired"]) 
            : OperationResult<User>.SuccessResult(user);
    }
    
    public async Task<OperationResult<User>> UserCredentialsAsync(string email, string password, bool rememberMe)
    {
        var emailExist = await userManager.FindByEmailAsync(email);
        if (emailExist == null)
            return OperationResult<User>.Failure([$"Email {email} does not exist."]);
        
        var checkCredential = await signInManager.CheckPasswordSignInAsync(emailExist, password, false);
        if (!checkCredential.Succeeded)
            return OperationResult<User>.Failure(["Incorrect password."]);

        emailExist.RememberMe = rememberMe;
        await userManager.UpdateAsync(emailExist);
        
        var role = await userManager.GetRolesAsync(emailExist);
        var user = emailExist.MapToUser();
        user.Role = string.Join(',', role);
        
        return OperationResult<User>.SuccessResult(user);
    }
    
    public async Task<OperationResult<User>> AddUserAsync(User user)
    {
        var emailExist = await userManager.FindByEmailAsync(user.Email!);
        if (emailExist != null) 
            return OperationResult<User>.Failure([$"Email {user.Email} already exists."]);
        
        var userEntity = new UserEntity { Name = user.Username, UserName = user.Email, Email = user.Email };
        
        var result = await userManager.CreateAsync(userEntity, user.Password!);
        if (!result.Succeeded)
            return OperationResult<User>.Failure(result.Errors.Select(e => e.Description).ToList());
        
        var roleResult = await userManager.AddToRoleAsync(userEntity, user.Role!);
        if (!roleResult.Succeeded)
        {
            await userManager.DeleteAsync(userEntity);
            return OperationResult<User>.Failure(roleResult.Errors.Select(e => e.Description).ToList());
        }
        
        return OperationResult<User>.SuccessResult(userEntity.MapToUser());
    }
    
    public async Task<OperationResult> AddRefreshTokenAsync(string userId, string refreshToken, string expiryDate)
    {
        var refreshTokenEntity = new RefreshTokenEntity
        {
            Id = Guid.NewGuid().ToString(),
            Token = refreshToken, 
            Expires = DateTime.UtcNow.AddDays(int.Parse(expiryDate)),
            IsUsed = false,
            IsRevoked = false,
            UserId = userId
        };
            
        await context.RefreshTokens.AddAsync(refreshTokenEntity);
        await context.SaveChangesAsync();
        
        return OperationResult.SuccessResult();
    }
    
    public async Task<OperationResult> UpdateRefreshTokenAsync(
        string oldRefreshToken, string newRefreshToken, string expiryDate)
    {
        var tokenExist = await context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == oldRefreshToken);
        if (tokenExist == null)
            return OperationResult.Failure(["Refresh token not found"]);
        
        tokenExist.Token = newRefreshToken;
        tokenExist.Expires = DateTime.UtcNow.AddDays(int.Parse(expiryDate));
        await context.SaveChangesAsync();
        
        return OperationResult.SuccessResult();
    }
    
    public async Task<OperationResult> UpdateUserAsync(User user)
    {
        var emailExist = await userManager.FindByEmailAsync(user.Email!);
        if (emailExist == null)
            return OperationResult.Failure([$"Email {user.Id} does not exist."]);

        //TODO: Think about what is actually should be updated

        var result = await userManager.UpdateAsync(emailExist);
        return !result.Succeeded 
            ? OperationResult.Failure(result.Errors.Select(e => e.Description).ToList()) 
            : OperationResult.SuccessResult();
    }
    
    public async Task<OperationResult> DeleteUserAsync(string userId)
    {
        var emailExist = await userManager.FindByEmailAsync(userId);
        if (emailExist == null)
            return OperationResult.Failure([$"Email {userId} does not exist."]);

        var result = await userManager.DeleteAsync(emailExist);
        return !result.Succeeded 
            ? OperationResult.Failure(result.Errors.Select(e => e.Description).ToList()) 
            : OperationResult.SuccessResult();
    }
}