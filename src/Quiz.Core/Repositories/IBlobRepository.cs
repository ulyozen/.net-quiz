using Microsoft.AspNetCore.Http;
using Quiz.Core.Common;

namespace Quiz.Core.Repositories;

public interface IBlobRepository
{
    Task<OperationResult<string>> UploadFileAsync(IFormFile file);
    
    Task<OperationResult<string>> GetFileUrlAsync(string fileName);
}