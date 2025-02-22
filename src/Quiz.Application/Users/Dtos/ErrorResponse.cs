using Quiz.Application.Abstractions;

namespace Quiz.Application.Users.Dtos;

public class ErrorResponse : BaseResponse
{
    public string Message { get; private set; }
    
    public List<string> Errors { get; private set; }

    public ErrorResponse(string message, List<string> errors)
    {
        Success = false;
        Message = message;
        Errors = errors;
    }
}