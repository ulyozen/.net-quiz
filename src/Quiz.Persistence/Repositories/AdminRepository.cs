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
    
    public async Task<PaginationResult<User>> GetUsersAsync(int page, int pageSize)
    {
        var totalCount = await _context.Users.CountAsync();
        
        var result = await _context.Users
            .Select(u => new
            {
                User = u,
                Roles = _context.UserRoles
                    .Where(ur => ur.UserId == u.Id)
                    .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
                    .ToList()
            })
            .OrderBy(u => u.User.Email)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        var users = result.Select(u => User.Restore(u.User.Id, u.User.Name, 
            u.User.Email, u.User.IsBlocked, u.Roles)).ToList();

        return PaginationResult<User>.Create(users, totalCount, page, pageSize);
    }
    
    public async Task<OperationResult> ChangeRoleAsync(string userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
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
        if (user is null)
            return OperationResult.Failure(DomainErrors.User.UserNotFound);
        
        if (user.IsBlocked)
            return OperationResult.Failure(DomainErrors.User.UserAlreadyBlocked);
        
        user.IsBlocked = true;
        
        var result = await _userManager.UpdateAsync(user);
        
        return result.Succeeded 
            ? OperationResult.SuccessResult()
            : OperationResult.Failure(result.Errors.Select(e => e.Description).ToList());
    }
    
    public async Task<OperationResult> UnblockUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            return OperationResult.Failure(DomainErrors.User.UserNotFound);
        
        if (!user.IsBlocked)
            return OperationResult.Failure(DomainErrors.User.UserAlreadyUnblocked);
        
        user.IsBlocked = false;
        
        var result = await _userManager.UpdateAsync(user);
        
        return result.Succeeded 
            ? OperationResult.SuccessResult()
            : OperationResult.Failure(result.Errors.Select(e => e.Description).ToList());
    }
    
    public async Task<OperationResult> DeleteUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            return OperationResult.Failure(DomainErrors.User.UserNotFound);
        
        var result = await _userManager.DeleteAsync(user);
        
        return result.Succeeded 
            ? OperationResult.SuccessResult()
            : OperationResult.Failure(result.Errors.Select(e => e.Description).ToList());
    }
}