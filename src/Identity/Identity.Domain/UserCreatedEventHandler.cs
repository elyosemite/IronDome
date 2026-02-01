using Identity.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Identity.Domain;

public class UserCreatedEventHandler(ILogger<UserCreatedEventHandler> logger)
    : INotificationHandler<UserCreatedEvent>
{
    public Task Handle(UserCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating user {userId}...", domainEvent.UserId);
        logger.LogInformation("Saving user {userName} in the imaginary database", domainEvent.Name);
        return Task.CompletedTask;
    }
}
