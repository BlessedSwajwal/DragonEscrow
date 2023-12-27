using Application.Common.Services;
using Domain.Order.Events;
using MediatR;

namespace Application.Users.Events;

public class OrderCreatedEventHandler(IUnitOfWork unitOfWork) : INotificationHandler<OrderCreatedEvent>
{
    public async Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        //Get the user
        var consumer = await unitOfWork.ConsumerRepository.GetByIdAsync(notification.order.ConsumerId);

        consumer.AddOrder(notification.order.Id);
    }
}
