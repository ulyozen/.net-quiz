using Quiz.Core.Common;
using Quiz.Core.Entities;

namespace Quiz.Core.Abstractions;

public interface IAdminRepository
{
    Task<IEnumerable<User>> GetUsersAsync();
    
    Task<OperationResult> ChangeRoleAsync(string userId, string role);
    
    Task<OperationResult> BlockUserAsync(string userId);
    
    Task<OperationResult> UnblockUserAsync(string userId);
    
    Task<OperationResult> DeleteUserAsync(string userId);
}