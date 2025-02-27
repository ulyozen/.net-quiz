using MediatR;

namespace Quiz.Core.Abstractions;

public interface IDomainEvent : INotification
{
    DateTime CreatedAt { get; }
}