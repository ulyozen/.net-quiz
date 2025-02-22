using FluentValidation;
using Quiz.Application.Common;
using Quiz.Application.Users.Commands.AdminActions;

namespace Quiz.Application.Users.Validators.AdminActions;

public class ChangeRoleValidator : AbstractValidator<ChangeRoleCommand>
{
    public ChangeRoleValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage(ApplicationErrors.Admin.UserIdRequired);
        
        RuleFor(command => command.Role)
            .NotEmpty().WithMessage(ApplicationErrors.Admin.RoleRequired);
    }
}