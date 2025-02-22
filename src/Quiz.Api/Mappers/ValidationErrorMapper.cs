using FluentValidation.Results;
using Quiz.Application.Common;
using Quiz.Application.Users.Dtos;

namespace Quiz.Api.Mappers;

public static class ValidationErrorMapper
{
    public static ErrorResponse MapToError(this ValidationResult validationResult)
    {
        return new ErrorResponse(
            ApplicationErrors.Common.ErrorResponse,
            validationResult.Errors.Select(e => e.ErrorMessage).ToList()
        );
    }
}