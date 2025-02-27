namespace Quiz.Core.Abstractions;

public interface IDomainEventDispatcher
{
    Task Dispatch(IEnumerable<IDomainEvent> domainEvents);
}