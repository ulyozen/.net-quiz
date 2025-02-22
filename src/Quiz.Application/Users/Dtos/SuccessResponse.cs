using Quiz.Application.Abstractions;

namespace Quiz.Application.Users.Dtos;

public class SuccessResponse<T> : BaseResponse
{
    public T? Data { get; private set; }
    
    public SuccessResponse(T? data)
    {
        Success = true;
        Data = data;
    }
}