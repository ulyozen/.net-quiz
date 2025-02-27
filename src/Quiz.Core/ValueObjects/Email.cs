using System.Text.RegularExpressions;

namespace Quiz.Core.ValueObjects;

public partial class Email
{
    public string Value { get; }
    
    private Email(string value) => Value = value;
    
    public static Email From(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        
        if (!IsValidRegex().IsMatch(value)) 
            throw new ArgumentException($"'{value}' is not a valid email address.", nameof(value));
        
        return new Email(value);
    }
    
    public override string ToString() => Value;
    
    public override bool Equals(object? obj)
    {
        if (obj is not Email email) return false;
        
        return Value.Equals(email.Value);
    }

    public override int GetHashCode() => Value.GetHashCode();
    
    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled)]
    private static partial Regex IsValidRegex();
}