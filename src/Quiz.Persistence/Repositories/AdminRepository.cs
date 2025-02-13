using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quiz.Core.Abstractions;
using Quiz.Core.Common;
using Quiz.Core.Entities;
using Quiz.Persistence.Common;
using Quiz.Persistence.Context;
using Quiz.Persistence.Entities;

namespace Quiz.Persistence.Repositories;

public class AdminRepository : IAdminRepository
{
    private readonly AppDbContext _context;
    private readonly UserManager<UserEntity> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    
    public AdminRepository(
        AppDbContext context, 
        UserManager<UserEntity> userManager, 
        RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }
    
    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        var usersWithRoles = await (
            from user in _context.Users
            join userRole in _context.UserRoles on user.Id equals userRole.UserId into userRoles
            from ur in userRoles.DefaultIfEmpty()
            join role in _context.Roles on ur.RoleId equals role.Id into roles
            from r in roles.DefaultIfEmpty()
            group r by user into g
            select new
            {
                User = g.Key,
                Roles = g.Where(role => role != null).Select(role => role.Name).ToList()
            }
        ).ToListAsync();
        
        return usersWithRoles.Select(u =>
        {
            var user = u.User.MapToUser();
            user.Role = string.Join(',', u.Roles);
            return user;
        });
    }
    
    public async Task<OperationResult> ChangeRoleAsync(string userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return OperationResult.Failure(DomainErrors.User.UserNotFound);
        
        var roleExists = await _roleManager.RoleExistsAsync(role);
        if (!roleExists)
            return OperationResult.Failure(DomainErrors.User.RoleNotFound);
        
        var currentRoles = await _userManager.GetRolesAsync(user);
        if (currentRoles.Contains(role))
            return OperationResult.Failure(DomainErrors.User.UserHasRole);
        
        var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
        if (!removeResult.Succeeded)
            return OperationResult.Failure(removeResult.Errors.Select(e => e.Description).ToList());
        
        var addRole = await _userManager.AddToRoleAsync(user, role);
        return !addRole.Succeeded 
            ? OperationResult.Failure(addRole.Errors.Select(e => e.Description).ToList()) 
            : OperationResult.SuccessResult();
    }
    
    public async Task<OperationResult> BlockUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return OperationResult.Failure(DomainErrors.User.UserNotFound);
        
        user.LockoutEnd = DateTimeOffset.UtcNow.AddYears(100);
        var result = await _userManager.UpdateAsync(user);
        
        return result.Succeeded 
            ? OperationResult.SuccessResult()
            : OperationResult.Failure(result.Errors.Select(e => e.Description).ToList());
    }
    
    public async Task<OperationResult> UnblockUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return OperationResult.Failure(DomainErrors.User.UserNotFound);
        
        user.LockoutEnd = null;
        var result = await _userManager.UpdateAsync(user);
        
        return result.Succeeded 
            ? OperationResult.SuccessResult()
            : OperationResult.Failure(result.Errors.Select(e => e.Description).ToList());
    }
    
    public async Task<OperationResult> DeleteUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return OperationResult.Failure(DomainErrors.User.UserNotFound);
        
        var result = await _userManager.DeleteAsync(user);
        
        return result.Succeeded 
            ? OperationResult.SuccessResult()
            : OperationResult.Failure(result.Errors.Select(e => e.Description).ToList());
    }
}