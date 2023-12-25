using Domain.Common;
using Domain.User;

namespace Domain.Order.Events;

public record OrderAcceptedEvent(Order order, UserId providerId) : IDomainEvent;
