using Quiz.Core.Common;
using Quiz.Core.Entities;

namespace Quiz.Core.Repositories;

public interface IAdminRepository
{
    Task<PaginationResult<User>> GetUsersAsync(int page, int pageSize);
    
    Task<OperationResult> ChangeRoleAsync(string userId, string role);
    
    Task<OperationResult> BlockUserAsync(string userId);
    
    Task<OperationResult> UnblockUserAsync(string userId);
    
    Task<OperationResult> DeleteUserAsync(string userId);
}