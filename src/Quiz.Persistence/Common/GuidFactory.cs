using Quiz.Application.Abstractions;

namespace Quiz.Persistence.Common;

public class GuidFactory :IGuidFactory
{
    public string Create() => Guid.NewGuid().ToString();
}