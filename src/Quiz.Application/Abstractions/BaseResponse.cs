namespace Quiz.Application.Abstractions;

public abstract class BaseResponse : IResponse
{
    public bool Success { get; set; }
}