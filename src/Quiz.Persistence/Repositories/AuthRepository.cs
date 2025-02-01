using Quiz.Application.Abstractions;
using Quiz.Core.Abstractions;
using Quiz.Core.Common;
using Quiz.Core.Entities;

namespace Quiz.Persistence.Repositories;

public class AuthRepository(IUserRepository repo) : IAuthRepository
{
    public async Task<OperationResult<User>> Create(User user)
    {
        var result = await repo.AddUserAsync(user);
        
        return !result.Success 
            ? OperationResult<User>.Failure(result.Errors!) 
            : OperationResult<User>.SuccessResult(result.Data!);
    }
    
    public async Task<OperationResult<User>> Login(string email, string password)
    {
        var result = await repo.UserCredentialsAsync(email, password);
        
        return !result.Success
            ? OperationResult<User>.Failure(result.Errors!) 
            : OperationResult<User>.SuccessResult(result.Data!);
    }
}