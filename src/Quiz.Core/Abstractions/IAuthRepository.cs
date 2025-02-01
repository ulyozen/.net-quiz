using Quiz.Core.Common;
using Quiz.Core.Entities;

namespace Quiz.Core.Abstractions;

public interface IAuthRepository
{
    Task<OperationResult<User>> Create(User user);
    
    Task<OperationResult<User>> Login(string email, string password);
}