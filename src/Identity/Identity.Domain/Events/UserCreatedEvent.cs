using Ardalis.SharedKernel;

namespace Identity.Domain.Events;

public class UserCreatedEvent(Guid userId, string name)
    : DomainEventBase
{
    public Guid UserId { get; init; } = userId;
    public string Name { get; init; } = name;
}