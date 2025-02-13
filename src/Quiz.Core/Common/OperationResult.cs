namespace Quiz.Core.Common;

public class OperationResult
{
    public bool Success { get; }
    
    public List<string>? Errors { get; }

    protected OperationResult(bool success, List<string>? errors = null)
    {
        Success = success;
        Errors = errors ?? [];
    }
    
    public static OperationResult SuccessResult() => new(true);
    
    public static OperationResult Failure(string error) => new(false, [error]);
    
    public static OperationResult Failure(List<string> errors) => new(false, errors);
}

public class OperationResult<T> : OperationResult
{
    public T? Data { get; }

    private OperationResult(bool success, T? data, List<string>? errors) 
        : base(success, errors)
    {
        Data = data;
    }
    
    public static OperationResult<T> SuccessResult(T data) => new(true, data, null);
    
    public new static OperationResult<T> Failure(string error) => new(false, default, [error]);
    
    public new static OperationResult<T> Failure(List<string> errors) => new(false, default, errors);
}