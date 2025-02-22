namespace Quiz.Core.Abstractions;

public interface IHasDomainEvent
{
    IReadOnlyCollection<IDomainEvent> GetDomainEvents();
    
    void ClearDomainEvents();
}