using Domain.Common;

namespace Domain.Order;

public record OrderCreatedEvent(Order order) : IDomainEvent;
