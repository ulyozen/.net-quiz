namespace Quiz.Core.Common;

public class OperationResult
{
    public bool Success { get; set; }
    
    public List<string>? Errors { get; set; }
    
    public static OperationResult SuccessResult() => new() { Success = true };
    
    public static OperationResult Failure(List<string> errors) => new() { Success = false, Errors = errors };
}

public class OperationResult<T> : OperationResult
{
    public T? Data { get; set; }
    
    public static OperationResult<T> SuccessResult(T data) => new() { Success = true, Data = data };
    
    public new static OperationResult<T> Failure(List<string> errors) => new() { Success = false, Errors = errors };
}