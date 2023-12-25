using Domain.Common;

namespace Domain.Order.Events;

public record OrderCreatedEvent(Order order) : IDomainEvent;
