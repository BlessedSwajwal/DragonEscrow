using Domain.Common;
using Domain.User;

namespace Domain.Order;

public record OrderAcceptedEvent(Order order, UserId providerId) : IDomainEvent;
