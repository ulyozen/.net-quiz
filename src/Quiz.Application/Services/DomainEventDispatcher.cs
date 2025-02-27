using MediatR;
using Quiz.Core.Abstractions;

namespace Quiz.Application.Services;

public class DomainEventDispatcher(IMediator mediator) : IDomainEventDispatcher
{
    public async Task Dispatch(IEnumerable<IDomainEvent> domainEvents)
    {
        foreach (var domainEvent in domainEvents)
        {
            await mediator.Publish(domainEvent);
        }
    }
}