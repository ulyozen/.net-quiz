using Microsoft.AspNetCore.Http;
using Quiz.Core.Common;
using Quiz.Core.Repositories;

namespace Quiz.MinIo.Repositories;

public class MinioRepository : IBlobRepository
{
    public Task<OperationResult<string>> UploadFileAsync(IFormFile file)
    {
        throw new NotImplementedException();
    }

    public Task<OperationResult<string>> GetFileUrlAsync(string fileName)
    {
        throw new NotImplementedException();
    }
}