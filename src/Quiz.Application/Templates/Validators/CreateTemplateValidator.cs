using FluentValidation;
using Quiz.Application.Common;
using Quiz.Application.Templates.Commands;

namespace Quiz.Application.Templates.Validators;

public class CreateTemplateValidator : AbstractValidator<CreateTemplateCommand>
{
    public CreateTemplateValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage(ApplicationErrors.Template.TitleRequired);
        
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage(ApplicationErrors.Template.DescriptionRequired);
        
        RuleFor(x => x.Topic)
            .NotEmpty().WithMessage(ApplicationErrors.Template.TopicRequired);
        
        RuleFor(x => x.AuthorId)
            .NotEmpty().WithMessage(ApplicationErrors.Template.AuthorIdRequired);
        
        RuleFor(x => x.AuthorName)
            .NotEmpty().WithMessage(ApplicationErrors.Template.AuthorNameRequired);
        
        RuleFor(x => x.IsPublic)
            .NotEmpty().WithMessage(ApplicationErrors.Template.IsPublicRequired);

        RuleFor(x => x.Questions)
            .NotEmpty();
    }
}