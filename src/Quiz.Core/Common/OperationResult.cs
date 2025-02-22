namespace Quiz.Core.Common;

public class OperationResult
{
    public bool Success { get; private set; }
    
    public string Message { get; private set; }
    
    public List<string> Errors { get; private set; }
    
    protected OperationResult(bool success, List<string> errors)
    {
        Success = success;
        Message = errors.FirstOrDefault() ?? string.Empty;
        Errors = errors;
    }
    
    public static OperationResult SuccessResult() => new(true, []);
    
    public static OperationResult Failure(string error) => new(false, [error]);
    
    public static OperationResult Failure(List<string> errors) => new(false, errors);
}

public class OperationResult<T> : OperationResult
{
    public T? Data { get; private set; }
    
    private OperationResult(bool success, T? data, List<string> errors) : base(success, errors)
    {
        Data = data;
    }
    
    public static OperationResult<T> SuccessResult(T data) => new(true, data, []);
    
    public new static OperationResult<T> Failure(string error) => new(false, default, [error]);
    
    public new static OperationResult<T> Failure(List<string> errors) => new(false, default, errors);
}