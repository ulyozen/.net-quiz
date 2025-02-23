using Quiz.Core.Common;

namespace Quiz.Core.Repositories;

public interface IBlobRepository
{
    Task<OperationResult<string>> UploadFileAsync(BlobObject file);
    
    Task<OperationResult<string>> GetFileUrlAsync(string fileName);
}