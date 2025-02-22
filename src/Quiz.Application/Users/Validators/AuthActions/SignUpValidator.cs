using FluentValidation;
using Quiz.Application.Common;
using Quiz.Application.Users.Commands.AuthActions;

namespace Quiz.Application.Users.Validators.AuthActions;

public class SignUpValidator : AbstractValidator<SignUpCommand>
{
    public SignUpValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage(ApplicationErrors.Auth.UsernameRequired);
        
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(ApplicationErrors.Auth.EmailRequired)
            .EmailAddress().WithMessage(ApplicationErrors.Auth.InvalidEmailFormat);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(ApplicationErrors.Auth.PasswordRequired)
            .MinimumLength(1).WithMessage(ApplicationErrors.Auth.PasswordTooShort);
    }
}