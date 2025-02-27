using MediatR;
using Quiz.Core.DomainEvents;

namespace Quiz.Application.Users.Handlers.Events;

public class UserCreatedEventHandler : INotificationHandler<UserCreatedEvent>
{
    public Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
    {
        /*
         * Send notification to user: mail, telegram, sms
         * await sendEmailAsync(notification.Email.Value, "Hello")
         * await sendTelegramAsync(notification.Phone.Value, "Hello")
         * await sendSmsAsync(notification.Phone.Value, "Hello")
         * Or
         * await sendNotificationAsync(notification)
         */
        
        throw new NotImplementedException();
    }
}