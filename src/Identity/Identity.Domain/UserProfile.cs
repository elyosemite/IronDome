using Ardalis.SharedKernel;
using Identity.Domain.Events;
using MediatR;

namespace Identity.Domain;

public class UserProfile : EntityBase<Guid>, IAggregateRoot
{
    public string Name { get; }
    private readonly List<DomainEventBase> _domainEvents = new();

    public UserProfile(string name)
    {
        Name = name;
        Id = Guid.NewGuid();
        RegisterDomainEvent(new UserCreatedEvent(Id, Name));
    }
    
    public static UserProfile Create(string name)
        => new UserProfile(name);
}
