using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;

namespace Quiz.Api.Configuration;

public class ValidateConfig<T> : IValidateOptions<T> where T : class, new()
{
    public ValidateOptionsResult Validate(string? name, T options)
    {
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(options);
        var isValid = Validator.TryValidateObject(options, validationContext, validationResults, true);
        
        if (isValid) return ValidateOptionsResult.Success;

        var errors = validationResults.Select(vr => vr.ErrorMessage).ToArray();
        return ValidateOptionsResult.Fail(errors!);
    }
}